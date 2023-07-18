const express = require('express');
const cors = require('cors');

const app = express();

// Enable CORS
app.use(cors({ origin: 'http://localhost:4200' }));

// Define your routes and other server configuration
app.get('/', (req, res) => {
  res.send('Hello, World!');
});

// Start the server
app.listen(3000, () => {
  console.log('Server is running on port 3000');
});