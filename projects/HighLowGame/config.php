<?php
//Database credentials
define('dbServerName','sql204.epizy.com');
define('dbUsername','epiz_30809342');
define('dbPassword','eJMnDdQ4J3Jnkm');
define('dbName','epiz_30809342_3750spr22');

    //Attempt to connect to MySQL database
    $link = mysqli_connect(dbServerName,
        dbUsername,
        dbPassword,
        dbName);

    if($link == false) die("Error: Could not connect. " . mysqli_connect_error());

