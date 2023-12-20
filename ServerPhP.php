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
  // ------------------------------------------------------------------------------ KILL
    if (isset($_POST["SessionID"]) && isset($_POST["RunID"]) && 
    isset($_POST["PlayerKiller_PositionX"]) && isset($_POST["PlayerKiller_PositionY"]) && isset($_POST["PlayerKiller_PositionZ"]) && 
    isset($_POST["EnemyDeath_PositionX"]) && isset($_POST["EnemyDeath_PositionY"]) && isset($_POST["EnemyDeath_PositionZ"]) && 
    isset($_POST["Time"])) {

      $sessionID = $_POST["SessionID"];
      $runID = $_POST["RunID"];
      $playerPositionX = $_POST["PlayerKiller_PositionX"];
      $playerPositionY = $_POST["PlayerKiller_PositionY"];
      $playerPositionZ = $_POST["PlayerKiller_PositionZ"];
      $enemyPositionX = $_POST["EnemyDeath_PositionX"];
      $enemyPositionY = $_POST["EnemyDeath_PositionY"];
      $enemyPositionZ = $_POST["EnemyDeath_PositionZ"];
      $time = $_POST["Time"];

      // HEAT MAP KILL DATA
      $stmt = $connection->prepare("INSERT INTO `HeatMap_Kill` (`SessionID`, `RunID`, 
                                                `PlayerKiller_PositionX`, `PlayerKiller_PositionY`, `PlayerKiller_PositionZ`, 
                                                `EnemyDeath_PositionX`, `EnemyDeath_PositionY`, `EnemyDeath_PositionZ`, 
                                                `Time`) VALUES (?, ?, ?, ?, ?)");

      $stmt->bind_param("sisss", $sessionID, $runID, $playerPositionX, $playerPositionY, $playerPositionZ, $enemyPositionX, $enemyPositionY, $enemyPositionZ, $time);

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
      
    }
    // ------------------------------------------------------------------------------ DEATH
    else if (isset($_POST["SessionID"]) && isset($_POST["RunID"]) && 
    isset($_POST["PlayerDeath_PositionX"]) && isset($_POST["PlayerDeath_PositionY"]) && isset($_POST["PlayerDeath_PositionZ"]) && 
    isset($_POST["EnemyKiller_PositionX"]) && isset($_POST["EnemyKiller_PositionY"]) && isset($_POST["EnemyKiller_PositionZ"]) && 
    isset($_POST["Time"])) {

      $sessionID = $_POST["SessionID"];
      $runID = $_POST["RunID"];
      $playerPositionX = $_POST["PlayerDeath_PositionX"];
      $playerPositionY = $_POST["PlayerDeath_PositionY"];
      $playerPositionZ = $_POST["PlayerDeath_PositionZ"];
      $enemyPositionX = $_POST["EnemyKiller_PositionX"];
      $enemyPositionY = $_POST["EnemyKiller_PositionY"];
      $enemyPositionZ = $_POST["EnemyKiller_PositionZ"];
      $time = $_POST["Time"];

      // HEAT MAP DEATH DATA
      $stmt = $connection->prepare("INSERT INTO `HeatMap_Death` (`SessionID`, `RunID`, 
                                                `PlayerDeath_PositionX`, `PlayerDeath_PositionY`, `PlayerDeath_PositionZ`, 
                                                `EnemyKiller_PositionX`, `EnemyKiller_PositionY`, `EnemyKiller_PositionZ`, 
                                                `Time`) VALUES (?, ?, ?, ?, ?)");

      $stmt->bind_param("sisss", $sessionID, $runID, $playerPositionX, $playerPositionY, $playerPositionZ, $enemyPositionX, $enemyPositionY, $enemyPositionZ, $time);

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
      
    }
    // ------------------------------------------------------------------------------ PATH
    else if (isset($_POST["SessionID"]) && isset($_POST["RunID"]) && 
    isset($_POST["Player_PositionX"]) && isset($_POST["Player_PositionY"]) && isset($_POST["Player_PositionZ"]) && 
    isset($_POST["Player_RotationX"]) && isset($_POST["Player_RotationY"]) && isset($_POST["Player_RotationZ"]) && 
    isset($_POST["Time"])) {

      $sessionID = $_POST["SessionID"];
      $runID = $_POST["RunID"];
      $playerPositionX = $_POST["Player_PositionX"];
      $playerPositionY = $_POST["Player_PositionY"];
      $playerPositionZ = $_POST["Player_PositionZ"];
      $playerRotationX = $_POST["Player_RotationX"];
      $playerRotationY = $_POST["Player_RotationY"];
      $playerRotationZ = $_POST["Player_RotationZ"];
      $time = $_POST["Time"];

      // HEAT MAP DEATH DATA
      $stmt = $connection->prepare("INSERT INTO `HeatMap_Path` (`SessionID`, `RunID`, 
                                                `PlayerDeath_PositionX`, `PlayerDeath_PositionY`, `PlayerDeath_PositionZ`, 
                                                `EnemyKiller_PositionX`, `EnemyKiller_PositionY`, `EnemyKiller_PositionZ`, 
                                                `Time`) VALUES (?, ?, ?, ?, ?)");

      $stmt->bind_param("sisss", $sessionID, $runID, $playerPositionX, $playerPositionY, $playerPositionZ, $playerRotationX, $playerRotationY, $playerRotationZ, $time);

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
