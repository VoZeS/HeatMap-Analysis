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
    
  }
  else 
  {
    //echo "Form no valido";
  }

  // CLOSE
  mysqli_close($connection);

}
?>
