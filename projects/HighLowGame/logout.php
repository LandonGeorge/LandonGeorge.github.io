<?php
session_destroy();

// Redirect user to login.php
header("location: index.php");
?>