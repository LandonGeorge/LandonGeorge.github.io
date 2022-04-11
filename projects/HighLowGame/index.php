<?php
//Include config file
require_once "config.php";

//start a session
session_start();

// Define variables and initialize with empty values
$username = $password = "";
$usernameError = $passwordError = $loginError = "";
$_SESSION["loggedin"] = false;

function verifyPassword($password,$hashedPassword, $passwordSalt){
    return((hash("sha256",$password . $passwordSalt)) == $hashedPassword);
}

// Processing form data when form is submitted
if($_SERVER["REQUEST_METHOD"] == "POST") {

    // Check if username is empty
    if (empty(trim($_POST["username"]))) {
        $usernameError = "Please enter username.";
    } else {
        $username = trim($_POST["username"]);
    }

    // Check if password is empty
    if (empty(trim($_POST["password"]))) {
        $passwordError = "Please enter your password.";
    } else {
        $password = trim($_POST["password"]);
    }

    // Validate credentials
    if (empty($usernameError) && empty($passwordError)) {
        // Prepare a select statement
        $sql = "SELECT user_id, username,password, password_salt FROM Users WHERE username = ?";
        if ($stmt = mysqli_prepare($link, $sql)) {
            // Bind variables to the prepared statement as parameters
            mysqli_stmt_bind_param($stmt, "s", $paramUsername);

            // Set parameters
            $paramUsername = $username;

            // Attempt to execute the prepared statement
            if (mysqli_stmt_execute($stmt)){
                // Store result
                mysqli_stmt_store_result($stmt);

                // Check if username exists, if yes then verify password
                if (mysqli_stmt_num_rows($stmt) == 1){

                    // Bind result variables
                    mysqli_stmt_bind_result($stmt, $id, $username, $hashedPassword, $passwordSalt);

                    if(mysqli_stmt_fetch($stmt)){

                        if(verifyPassword($password, $hashedPassword, $passwordSalt)){
                            //set username as a session variable
                            $_SESSION["username"] = $username;
                            $_SESSION["loggedin"] = true;
                            // Redirect user to game page menu
                            header("location: gameMenu.php");
                        }else{
                            // Password is not valid, display a generic error message
                            $loginError = "Invalid username or password.";
                        }
                    }else{
                        $loginError = "failed to fetch sql statement";
                    }
                }else{
                    // Username doesn't exist, display a generic error message
                    $loginError = "Invalid username or password.";
                }
            } else {
                echo "Oops! Something went wrong. Please try again later.";
            }

            // Close statement
            mysqli_stmt_close($stmt);
        }
    }
}

// Close connection
mysqli_close($link);
?>
<!DOCTYPE html>
<html lang="en">
<head>
    <title>Login</title>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@4.6.1/dist/css/bootstrap.min.css">
    <script src="https://cdn.jsdelivr.net/npm/jquery@3.5.1/dist/jquery.slim.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/popper.js@1.16.1/dist/umd/popper.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@4.6.1/dist/js/bootstrap.bundle.min.js"></script>
</head>
<body>
    <div class="jumbotron">
        <h1 class="display-4">Log in or Sign up to continue.</h1>
    </div>
    <?php
    if(!empty($loginError)){
        echo '<div class="alert alert-danger">' . $loginError . '</div>';
    }
    ?>
    <div class="container">
        <form action="<?php echo htmlspecialchars($_SERVER["PHP_SELF"]); ?>" method="post">
            <div class="form-group">
                <label for="username">Username</label><br>
                <input class="form-control <?php echo (!empty($usernameError)) ? 'is-invalid' : ''; ?>" value="<?php echo $username; ?>" type="text" name="username"/>
                <span class="invalid-feedback"><?php echo $usernameError; ?></span>
            </div>
            <div class="form-group">
                <label for="password">Password</label><br>
                <input class="form-control <?php echo (!empty($passwordError)) ? 'is-invalid' : ''; ?>" type="password" name="password"/>
                <span class="invalid-feedback"><?php echo $passwordError; ?></span>
            </div>
            <a href="highLowGamePage.php"><button class="btn btn-primary" type="submit">Log In</button></a>

            <p>Don't have an account? <a class ="mt-2" href="createAccount.php">Sign up now</a></p>
        </form>
    </div>
</body>
</html>