<?php
//Include config file
require_once "config.php";

//Start session
session_start();

if(!($_SESSION["loggedin"] == 1)){
    // Redirect user to log in if they aren't logged in and try to go to the high scores page
    header("location: index.php");
    exit;
}



?>
<!DOCTYPE html>
<html>
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
                <a class="nav-link" href="gameMenu.php">Play Again</a>
            </li>
            <li class="nav-item">
                <a class="nav-link" href="logout.php">Logout</a>
            </li>
        </ul>
    </div>
</nav>
<div class="card">
    <div class="card-body">
        <div class="container">
            <h1 class="card-header">High Scores</h1>
            <h3 class="card-subtitle">
                <?php
                if($_SESSION["played"] == true){
                echo "You guessed the number in " . $_SESSION["numGuesses"] . " tries " . $_SESSION["username"] . "!!";
                }?></h3>
            <table class="table">
                <thead>
                    <tr>
                        <th scope="col">#</th>
                        <th scope="col">Username</th>
                        <th scope="col">Guesses</th>
                        <th scope="col">Submitted On</th>
                    </tr>
                </thead>
                <tbody>
                    <?php
                        //Prepare select statement
                        $sql = "SELECT username,score,timestamp FROM HighScores ORDER BY score ASC LIMIT 10";
                        $result = mysqli_query($link,$sql);
                        $count = 1;
                        while($highScoreData = mysqli_fetch_array($result)){
                            echo "
                                    <tr>
                                        <th scope='row'>" . $count . "</th>
                                        <td>" . $highScoreData['username'] . "</td>
                                        <td>" . $highScoreData['score']."</td>
                                        <td>" . $highScoreData['timestamp'] . "</td>
                                    </tr>
                                 ";
                            $count++;
                        }
                    ?>
                </tbody>
            </table>
        </div>
    </div>
</div>

</body>
</html>
