<?php
require("../config.php");
include("../src/db.php");
include("../src/signature.php");


$path = parse_url($_SERVER['REQUEST_URI'], PHP_URL_PATH); 
$method = $_SERVER['REQUEST_METHOD'];
header('Content-Type: application/json; charset=utf-8'); // we always send back json

$config = get_config();
$db = new Database($config);


if ($path == '/scores')
{
  if ($method == 'GET')
  {
    echo json_encode([
      'scores' => $db->getTop()
    ]);
  }
  elseif ($method == 'POST')
  {
    $data = json_decode(file_get_contents('php://input'), true);
    if (!is_array($data))
    {
      http_response_code(400);
      exit;
    }

    // validate json input
    $initials = $data['initials'] ?? null;
    $score = $data['score'] ?? null;
    $nonce = $data['nonce'] ?? null;
    $signature = $data['signature'] ?? null;
    if (!is_string($initials) || !ctype_alnum($initials) || strlen($initials) != 3)
    {
      http_response_code(400);
      exit;
    }
    if (!is_int($score) || $score < 0)
    {
      http_response_code(400);
      exit;
    }
    if (!is_string($nonce) || strlen($nonce) !== 32 || !ctype_xdigit($nonce)
        || !is_string($signature) || strlen($signature) !== 64 || !ctype_xdigit($signature))
    {
      http_response_code(400);
      exit;
    }

    // check nonce and signature
    $replay = $db->nonceExists($nonce);
    if ($replay)
    {
      http_response_code(400);
      exit;
    }

    $valid = Signature::verify($initials, $score, $nonce, $signature, $config['secret']);
    if (!$valid)
    {
      http_response_code(400);
      exit;
    }

    // perform insertion and get data to return
    $db->recordNonce($nonce);
    $initials = strtoupper($initials);
    $id = $db->insert($initials, $score);
    $rank = $db->rankFor($id);
    $scores = $db->getTop();

    // send success response!
    echo json_encode([
      'accepted' => true,
      'rank' => $rank,
      'scores' => $scores,
    ]);
  }
  else
  {
    http_response_code(404);
  }
}
else
{
  http_response_code(404);
}

