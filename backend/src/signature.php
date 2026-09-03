<?php
  declare(strict_types=1); // don't allow type coercion, 

  final class Signature
  {
    static function verify(string $initials, int $score, string $nonce, string $signature, string $secret): bool
    {
      $payload = "$initials:$score:$nonce";
      $expected = hash_hmac('sha256', $payload, $secret);
      return hash_equals($expected, $signature);
    }
  }

