<?php
//Include config file
require_once "config.php";

//start a session
session_start();


// Define variables and initialize with empty values
$username = $password = $confirmPassword = "";
$usernameError = $passwordError = $confirmPasswordError = "";

//Generates a random 32 length salt string
function generateRandomSalt(){
    $characters = '0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ';
    $charactersLength = strlen($characters);
    $randomSalt = '';
    for ($i = 0; $i < 10; $i++) {
        $randomSalt .= $characters[rand(0, $charactersLength - 1)];
    }
    return $randomSalt;
}


// Processing form data when create account form is submitted
if($_SERVER["REQUEST_METHOD"] == "POST"){

    //Validate username
    if(empty(trim($_POST["username"]))){
        $usernameError = "Please enter a username";
    }elseif(!preg_match('/^[a-zA-Z0-9_]+$/', trim($_POST["username"]))){
        $usernameError = "Username can only contain letters, numbers, and underscores";
    }else{
        //Prepare a select statement
        $sql = "SELECT user_id FROM Users WHERE username = ?";
        if($stmt = mysqli_prepare($link, $sql)){

            //Bind variables to the prepared statement as parameters
            mysqli_stmt_bind_param($stmt, "s", $paramUsername);

            //Set parameters
            $paramUsername = trim($_POST["username"]);

            //Attempt to execute the prepared statement
            if(mysqli_stmt_execute($stmt)){

                //store result
                mysqli_stmt_store_result($stmt);

                if(mysqli_stmt_num_rows($stmt) == 1){
                    $usernameError = "This username is already taken.";
                }else{
                    $username = trim($_POST["username"]);
                }
            }else{
                echo "Oops! Something went wrong. Please try again later.";
            }

            // Close statement
            mysqli_stmt_close($stmt);
        }
    }

    // Validate password
    if(empty(trim($_POST["password"]))){
        $passwordError = "Please enter a password.";
    }elseif(strlen(trim($_POST["password"])) < 6){
        $passwordError = "Password must have atleast 6 characters.";
    }else{
        $password = trim($_POST["password"]);
    }

    // Validate confirm password
    if(empty(trim($_POST["confirmPassword"]))){
        $confirmPasswordError = "Please confirm password.";
    }else{
        $confirmPassword = trim($_POST["confirmPassword"]);
        if(empty($passwordError) && ($password != $confirmPassword)){
            $confirmPasswordError = "Password did not match.";
        }
    }


    // Check input errors before inserting in database
    if(empty($usernameError) && empty($passwordError) && empty($confirmPasswordError)){

        // Prepare an insert statement
        $sql = "INSERT INTO Users (username,password,password_salt) VALUES (?, ?,?)";

        if($stmt = mysqli_prepare($link, $sql)){
            // Bind variables to the prepared statement as parameters
            mysqli_stmt_bind_param($stmt,
                "sss",
                $paramUsername,
                $paramPassword,
                $paramPasswordSalt);

            // Set parameters
            $param_username = $username;
            $paramPasswordSalt = generateRandomSalt();
            $paramPassword = hash("sha256",$password . $paramPasswordSalt); // Creates a sha256 password hash based off users password + hash

            // Attempt to execute the prepared statement
            if(mysqli_stmt_execute($stmt)){
                // Redirect to login page
                header("location: index.php");
            } else{
                echo "Oops! Something went wrong. Please try again later. Error: $stmt->error";
            }

            // Close statement
            mysqli_stmt_close($stmt);
        }
    }
}

?>
<!DOCTYPE html>
<head>
<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@4.6.1/dist/css/bootstrap.min.css">
<script src="https://cdn.jsdelivr.net/npm/jquery@3.5.1/dist/jquery.slim.min.js"></script>
<script src="https://cdn.jsdelivr.net/npm/popper.js@1.16.1/dist/umd/popper.min.js"></script>
<script src="https://cdn.jsdelivr.net/npm/bootstrap@4.6.1/dist/js/bootstrap.bundle.min.js"></script>
</head>
	<body>
		<div class="jumbotron">
			<h1 class="display-4">Sign up to create your account</h1>
		</div>
		<div class="container">
			<form action="<?php echo htmlspecialchars($_SERVER["PHP_SELF"]); ?>" method="post">
				<div class="form-group">
					<label for="username">Username</label>
					<input class="form-control <?php echo (!empty($usernameError)) ? 'is-invalid' : ''; ?>" value="<?php echo $username; ?>" type="text" name="username"/>
                    <span class="invalid-feedback"><?php echo $usernameError; ?></span>
				</div>
				<div class="form-group">
					<label for="password">Password</label>
					<input class="form-control <?php echo (!empty($passwordError)) ? 'is-invalid' : ''; ?>" value="<?php echo $password; ?>" type="password" name="password"/>
                    <span class="invalid-feedback"><?php echo $passwordError; ?></span>
				</div>
				<div class="form-group">
					<label for="confirmPassword">Confirm Password</label>
					<input class="form-control"type="password" name="confirmPassword"/>
				</div>
				<button class="btn btn-primary" type="submit">Create Account</button>
			</form>
            <p>Already have an account? <a class ="mt-2" href="index.php">Log In</a>.</p>
		</div>
	</body>
</html>