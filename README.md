RestaurantsAPI

API REST para gestionar restaurantes en Colombia
Endpoints disponibles

    /api/Restaurant: Obtener todos los restaurantes.
    /api/Restaurant/{id}: Obtener un restaurante por su ID.
    /api/Restaurant/byCiudad/{ciudad}: Obtener todos los restaurantes de una ciudad específica.
    /api/Comentario: Obtener todos los comentarios de los restaurantes.
    /api/Comentario/ByRestauranteId/{restauranteId}: Obtener todos los comentarios de un restaurante específico.
    /api/Comentario/agregar-comentario: Agregar un nuevo comentario a un restaurante.

Formato de las solicitudes y respuestas

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
