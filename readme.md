# SpoofMess

## What is it?
A cross-platform messenger with any type of chats, stickers/emoji - that's just a dream for now.


### Project possibilities
Now you can registration and authorize in app, and also create you're chat

### Features
Soon you can add members to you're chat and connect to other user chats, and also send text messages

## What stage of the project at?
### While on Alpha 0.0.6 Realized first prototypes of:
- Common services:
    - Data services(db, cache, cached-aside repository);
    - Logger service(file, console loggers);
    - RabbitMQ service

- Microservice of entrance(SpoofEntranceService):
    - Db models services
    - Hash passwords and keys.
    - Publisher/Consumer services
    - model validators

- Microservice of settings(SpoofSettingsService):
    - Db models services
    - DTO setters
    - Publisher/Consumer services
    - model validators

- Microservice of files(SpoofFileService):
    - Db models services
    - Publisher/Consumer services
    - model validators

- Microservice of message(SpoofMessageService):
    - Db models services
    - Publisher/Consumer services
    - model validators