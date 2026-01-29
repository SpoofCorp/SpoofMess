# SpoofMess

## What is it?
A cross-platform messenger with any type of chats, stickers/emoji - that's just a dream for now.

## What stage of the project at?
### While on Alpha 0.0.5. Realized first prototypes of:
- Common services:
    - Data services(db, cache, cached-aside repository);
    - Logger service(file, console loggers);
    - RabbitMQ service

- Microservice of entrance(SpoofEntranceService):
    - Db models services
    - Hash passwords and keys.
    - Publisher/Consumer services

- Microservice of settings(SpoofSettingsService):
    - Db models services
    - DTO setters
    - Publisher/Consumer services

### Now this is a simply example only for test entrance functions
