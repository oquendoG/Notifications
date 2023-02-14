# Notifications
Pequeño software para desplegar imágenes como notificaciones.

Permite por medio de una gpo configurada en windows server mostrar imágenes con información
relevante para una empresa, cada día una imagen diferente, es un alternativa a rotar el fondo de pantalla y se debe configurar para que se inicie con windows.

En la carpeta de usuario C:\Usuarios\[Nombre del usuario] se debe crear una carpeta que se llame exactamente NotificationsImg, acá se ponen las
fotos nombradas del 1 al número de fotos que hayan, es decir 1.jpg, 2.jpg, 3.jpg, etc para que el software las muestre en ese orden especifico.
Las fotos deben estar nombradas así de lo contrario no las reconocerá.

El tamaño de las fotos es idealmente de 700px alto * 500px ancho; este tamaño es para que 
en las pantallas de 1366 * 768 no se desborde.

Acepta jpg y png como formatos pero solo uno a la vez, no mezclados.

En el primer inicio del programa el creará la carpeta mencionada, sino ha sido creada, y en la carpeta documentos del usuario actual, se creará un archivo
de texto llamado Savings.txt que que guardará el número de la imagen siguiente que mostrará la siguiente vez que se abra.
