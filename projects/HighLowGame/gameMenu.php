<?php
//start a session
session_start();

if(!($_SESSION["loggedin"] == 1)){
    // Redirect user to log in if they aren't logged in and try to go to the game menu
    header("location: index.php");
    exit;
}

unset($_SESSION["played"]);
?>
<!DOCTYPE html>
<head>
<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@4.6.1/dist/css/bootstrap.min.css">
<script src="https://cdn.jsdelivr.net/npm/jquery@3.5.1/dist/jquery.slim.min.js"></script>
<script src="https://cdn.jsdelivr.net/npm/popper.js@1.16.1/dist/umd/popper.min.js"></script>
<script src="https://cdn.jsdelivr.net/npm/bootstrap@4.6.1/dist/js/bootstrap.bundle.min.js"></script>
</head>
<body>
<nav class="navbar navbar-expand navbar-dark bg-dark">
    <div class="container-fluid">
        <a class="navbar-brand">Higher Lower</a>
        <ul class="navbar-nav me-2 mb-2">
            <li class="nav-item">
                <a class="nav-link" href="highScores.php">High Scores</a>
            </li>
            <li class="nav-item">
                <a class="nav-link" href="logout.php">Logout</a>
            </li>
        </ul>
    </div>
</nav>
    <div class="card">
        <div class="card-body">
            <div class="jumbotron text-center">
                <h1>Welcome <?php echo $_SESSION["username"]; ?> to High and Low!</h1>
                <h2>In this game you will try and guess the correct number between 1 and 100 in the least amount of guesses, if you guess correctly the game will end and the amount of guesses it took will be displayed.</h2>
            </div>
            <div class="container col-md-8 offset-md-2">
                <a href="game.php"><button class="btn btn-primary" type="button">Begin Game</button></a>
            </div>
        </div>
    </div>
</body>
</html>