# MelkYab

----
## Database Tables

- **Users**: Stores user information like unique ID, email, hashed password, full name, phone number, role (admin or user), and account creation date.
- **Properties**: Contains accommodation details including ID, title, description, type (villa, suite, etc.), capacity, number of rooms and beds, price per night, address, active status, and owner ID.
- **PropertyImages**: Holds images for properties with image ID, property ID, image URL, and whether it’s the cover image.
- **Amenities**: List of available amenities with ID and name.
- **PropertyAmenities**: Junction table linking amenities to properties by their IDs.
- **Bookings**: Records reservations with booking ID, user ID, property ID, check-in/out dates, number of guests, total price, status, and creation date.
