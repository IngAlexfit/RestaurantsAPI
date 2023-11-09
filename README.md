# 🍛 API de Restaurantes de Colombia

Esta API permite acceder a información sobre restaurantes colombianos, incluyendo detalles como ubicación, tipos de cocina, menús y comentarios de los clientes.

## Endpoints

### Autenticación

- `POST /api/Auth/login` - Inicia sesión y devuelve un token de autenticación

### Restaurantes

- `GET /api/Restaurant` - Obtiene todos los restaurantes
- `GET /api/Restaurant/{id}` - Obtiene un restaurante por ID
- `GET /api/Restaurant/byCiudad/{ciudad}` - Filtra restaurantes por ciudad

### Comentarios

- `GET /api/Comentario/ByRestauranteId/{restauranteId}` - Obtiene los comentarios de un restaurante
- `POST /api/Comentario/agregar-comentario` - Agrega un nuevo comentario
- `POST /api/Comentario/IncrementarLike` - Incrementa los "Me gusta" de un comentario
- `POST /api/Comentario/DarDisklike` - Incrementa los "No me gusta" de un comentario

## Modelos

### Restaurante

- `_Id` - ID del restaurante 
- `idRestaur` - ID numérico del restaurante
- `nombre` - Nombre del restaurante
- `ubicacion` - Ciudad donde se encuentra
- `tipoCocina` - Tipo de cocina (mexicana, italiana, etc)
- `descripcion` - Descripción del restaurante
- `imagenUrl` - URL de la imagen destacada
- `telefono` - Teléfono de contacto
- `horario` - Horario de atención
- `likes` - Cantidad de "Me gusta" 
- `visitas` - Cantidad de visitas

### Comentario

- `id` - ID del comentario
- `autor` - Nombre del autor del comentario
- `contenido` - Texto del comentario
- `likes` - Cantidad de "Me gusta" 
- `dislikes` - Cantidad de "No me gusta"
- `restaurante_id` - ID del restaurante relacionado
- `fecha` - Fecha de creación del comentario

### Autenticación

- `username` - Nombre de usuario
- `password` - Contraseña 

¡Pruébala y comenta tus restaurantes favoritos! ✨

Todas las solicitudes y respuestas de la API están formateadas en JSON.
Ejemplo de solicitud

Para obtener todos los restaurantes, se puede realizar la siguiente solicitud HTTP:

GET http://localhost:5000/api/Restaurant


## Ejemplo de respuesta

La siguiente es una respuesta de ejemplo para la solicitud anterior:

```json
[
  {
    "_Id": "632b80c360142f9b01646678",
    "idRestaur": 1,
    "nombre": "Restaurante El Patio",
    "ubicacion": "Bogotá",
    "tipoCocina": "Colombiana",
    "descripcion": "Un restaurante de comida tradicional colombiana, con un ambiente cálido y acogedor.",
    "imagenUrl": "https://example.com/restaurante-el-patio.jpg",
    "telefono": "+57 1 234 5678",
    "horario": "Lunes a domingo de 12:00 a 3:00 p.m. y de 7:00 p.m. a 10:00 p.m.",
    "likes": 100
  },
  {
    "_Id": "632b80c360142f9b01646679",
    "idRestaur": 2,
    "nombre": "Restaurante Andrés Carne de Res",
    "ubicacion": "Chía",
    "tipoCocina": "Parrilla",
    "descripcion": "Uno de los restaurantes más famosos de Colombia, con una amplia carta de carnes, mariscos y platos típicos.",
    "imagenUrl": "https://example.com/restaurante-andres-carne-de-res.jpg",
    "telefono": "+57 1 456 7890",
    "horario": "Lunes a domingo de 12:00 m. a 12:00 a.m.",
    "likes": 200
  }
]
