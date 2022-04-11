<?php
//Require config file for database connection
require_once "config.php";

//start a session
session_start();

if(!($_SESSION["loggedin"] == 1)){
    // Redirect user to log in if they aren't logged in and try to go to the game page
    header("location: index.php");
    exit;
}


//Initialize/set variables
$userGuessError = $userGuessFeedback = "";
$userGuess = 0;
$username = $_SESSION["username"];

//Set random number user has to guess
if(!isset($_SESSION["numberGoal"])){
    $_SESSION["numberGoal"] = rand(1,100);
    $_SESSION["numGuesses"] = 0;
}

//Process user input
if($_SERVER["REQUEST_METHOD"] == "POST"){
    // Check if input is empty
    if((empty(trim($_POST["userGuess"]))) || !(trim($_POST["userGuess"]) <= 100 && trim($_POST["userGuess"]) > 0) || strpos(trim($_POST["userGuess"]),".")){
        $userGuessError = "Please enter a whole number between 1 and 100";
    }else{
        $userGuess = trim($_POST["userGuess"]);
    }

    if($userGuess < $_SESSION["numberGoal"]){
        $userGuessFeedback = "Number is Higher than " . $userGuess . "!";
        $_SESSION["numGuesses"]++;
    }else if($userGuess > $_SESSION["numberGoal"]){
        $userGuessFeedback = "Number is Lower than " . $userGuess . "!";
        $_SESSION["numGuesses"]++;
    }else if($userGuess == $_SESSION["numberGoal"]){
        $_SESSION["numGuesses"]++;
        if(empty($userGuessError)){
            //prepare insert statement
            $sql = "INSERT INTO HighScores (username,score,timestamp) VALUES (?,?,?)";

            if($stmt = mysqli_prepare($link, $sql)){
                mysqli_stmt_bind_param($stmt,
                    "sis",
                    $paramUsername,
                    $paramScore,
                          $paramTimestamp);

                //set parameters
                $paramUsername = $_SESSION["username"];
                $paramScore = $_SESSION["numGuesses"];
                $paramTimestamp = date("Y-m-d H:i:s");


                // Attempt to execute INSERT INTO HighScores (username,score,timestamp) VALUES (?, ?,CURRENT_TIMESTAMP)
                if(mysqli_stmt_execute($stmt)){
                    //clear numberGoal to get ready for the player to play again
                    unset($_SESSION["numberGoal"]);
                    $_SESSION["played"] = true;
                    // Redirect user to game page menu
                    header("location: highScores.php");
                }else{
                    unset($_SESSION["numberGoal"]);
                    echo "Oops! Something went wrong. Please try again later.";
                }
            }else{
                echo "prepare failed";
            }
        }
    }

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
            <div class="container">
                <form action="" method="post">
                    <p class="form-group">
                        <label for="userGuess">Enter your guess (1-100)</label>
                    <p class="font-weight-bold"><?php echo $userGuessFeedback; ?></p>
                    <input class="form-control" type="number" name="userGuess" autofocus="autofocus"/>
                    <span class="invalid-feedback"><?php echo $userGuessError; ?></span>
                </form>
            </div>
        </div>
    </div>

</body>
</html>