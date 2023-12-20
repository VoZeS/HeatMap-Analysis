<?php
// Data base connection
$server = "localhost";
$user = "davidbo5";
$password = "9kPnax8NCqjb";
$dbname = "davidbo5";

$connection = mysqli_connect($server, $user, $password, $dbname);

if (!$connection) {
  die("Error al conectar a la base de datos: " . mysqli_connect_error());
}

if ($_SERVER["REQUEST_METHOD"] === "POST") {
  // ------------------------------------------------------------------------------ USER
    if (isset($_POST["Name"]) && isset($_POST["Age"]) && isset($_POST["Gender"]) && isset($_POST["Country"]) && isset($_POST["Date"])) {

      $name = $_POST["Name"];
      $age = $_POST["Age"];
      $gender = $_POST["Gender"];
      $country = $_POST["Country"];
      $date = $_POST["Date"];

      // ---------------------------------------------------------- NEW CODE (ACTUALLY WORKING) ----------------------------------------------------------

      // Consulta preparada
      $stmt = $connection->prepare("INSERT INTO `User` (`Username`, `Age`, `Gender`, `Country`, `FirstLoginDate`) VALUES (?, ?, ?, ?, ?)");
      $stmt->bind_param("sisss", $name, $age, $gender, $country, $date);

      if ($stmt->execute()) {
          // Obtiene la última ID generada
          $printID = $stmt->insert_id;
    
          // Imprime la última ID generada
         echo $printID;
      }  else {
          // Manejar errores de inserción
         echo "Error al crear el registro: " . $stmt->error;
      }

      // Cerrar la declaración
      $stmt->close();
      
      // --------------------------------------------------------------------------------------------------------------------

    } 
    // ------------------------------------------------------------------------------
    
    // ------------------------------------------------------------------------------ SESSION

    else if (isset($_POST["userID"]) && isset($_POST["startSessionTime"])) {
      $startSessionTime = $_POST["startSessionTime"];
      $userID = $_POST["userID"];
    
      // INSERT INTO
      $sql = "INSERT INTO `Sessions`(`Start`, user_id) VALUES ('$startSessionTime', '$userID')";

      //echo "Form2 valido";

    if ($connection->query($sql) === TRUE) {
      // Obtiene la ultima ID generada
      $printID = $connection->insert_id;
  
      // Imprime la ultima ID generada
      echo $printID;
    } else {
      // Manejar errores de insercion
      //echo "Error al crear el registro: " . $connection->error;
    }
    // ------------------------------------------------------------------------------
  }
  // ------------------------------------------------------------------------------ SHOPPING
  else if (isset($_POST["moneySpent"]) && isset($_POST["buyTime"]) && isset($_POST["userID"]) && isset($_POST["sessionID"])) {
    $moneySpent = $_POST["moneySpent"];
    $buyTime = $_POST["buyTime"];
    $userID = $_POST["userID"];
    $sessionID = $_POST["sessionID"];
  
    // INSERT INTO
    $sql = "INSERT INTO `Shopping` (`User-ID`, `Session-ID`, `Date`, `MoneySpend`) VALUES ('$userID', '$sessionID' , '$buyTime', '$moneySpent')";

    //echo "Form3 valido";

  if ($connection->query($sql) === TRUE) {
    // Obtiene la ultima ID generada
    $printID = $connection->insert_id;

    // Imprime la ultima ID generada
    echo $printID;
  } else {
    // Manejar errores de insercion
    //echo "Error al crear el registro: " . $connection->error;
  }
  // ------------------------------------------------------------------------------ END SESSION
  }
  else if (isset($_POST["endSessionTime"]) && isset($_POST["sessionID"])) {
    $endSessionTime = $_POST["endSessionTime"];
    $sessionID = $_POST["sessionID"];

    // ADD END SESSION TIME
    $sql = "UPDATE `Sessions` SET `End` = '$endSessionTime' WHERE `SessionID` = '$sessionID'";

    //echo "Form4 valido";

    if ($connection->query($sql) === TRUE) {
      // Obtiene la ultima ID generada
      $printID = $connection->insert_id;
  
      // Imprime la ultima ID generada
      echo $printID;
    } else {
      // Manejar errores de insercion
      //echo "Error al crear el registro: " . $connection->error;
    }

  }
  else 
  {
    //echo "Form no valido";
  }

  // CLOSE
  mysqli_close($connection);

}
?>
