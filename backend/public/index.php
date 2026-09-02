<?php
  require("../config.php");
  include("../src/db.php");

  $config = get_config();
  $db = new Database($config);

?>
