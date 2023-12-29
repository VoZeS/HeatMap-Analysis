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

if ($_SERVER["REQUEST_METHOD"] === "GET") {
    // HEAT MAP KILL DATA
    $sql = "SELECT * FROM HeatMap_Kill";

    $result = mysqli_query($connection, $sql);

    if ($result) {
        $data = array();

        while ($row = mysqli_fetch_assoc($result)) {
            $data[] = $row;
        }

        // Convertir datos a formato JSON y enviarlos a Unity
        echo json_encode($data);
    } else {
        echo "Error al ejecutar la consulta: " . mysqli_error($connection);
    }
}

// CLOSE
mysqli_close($connection);
?>