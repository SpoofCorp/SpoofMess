create extension "uuid-ossp";

create table "User"
(
	"Id" uuid constraint "PK_User_Id" primary key,
	"WasOnline" timestamp not null default CURRENT_TIMESTAMP,
	"Name" varchar(100) not null,
	"MonthsBeforeDelete" int not null default 6,
	"SearchMe" boolean not null default true,
	"ShowMe" boolean not null default true,
	"ForwardMessage" boolean not null default true,
	"InviteMe" boolean not null default true,
	"IsDeleted" boolean not null default false,
);

create table "ChatType"
(
    "Id" serial constraint "PK_ChatType_Id" primary key,
    "Name" varchar(50) not null unique
);

create table "ChatProperty"
(
    "Id" smallserial constraint "PK_ChatProperty_Id" primary key,
    "Name" varchar(50) not null,
    "Description" varchar(100)
);

create table "ChatTypeChatProperty"
(
    "ChatTypeId" int constraint "FK_ChatTypeChatProperty_ChatTypeId" references "ChatType"("Id"),
    "ChatPropertyId" smallint "FK_ChatTypeChatProperty_ChatPropertyId" references "ChatProperty"("Id"),
    constraint "PK_ChatTypeChatProperty_Id" primary key("ChatTypeId", "ChatPropertyId")
);

create table "Role"
(
    "Id" serial constraint "PK_Role_Id" primary key,
    "Name" varchar(50) not null unique,
);

create table "Permission"
(
    "Id" smallserial constraint "PK_Permission_Id" primary key,
    "Name" varchar(50) not null,
    "Description" varchar(100)
);

create table "RolePermission"
(
    "RoleId" int constraint "FK_RolePermission_RoleId" references "Role"("Id"),
    "PermissionId" smallint "FK_RolePermission_PermissionId" references "Permission"("Id"),
    constraint "PK_RolePermission_Id" primary key("RoleId", "PermissionId")
);

create table "Chat"
(
    "Id" uuid constraint "PK_Chat_Id" primary key,
    "ChatTypeId" bigint not null constraint "FK_Chat_ChatTypeId" references "ChatType"("Id"),
	"OwnerId" bigint constraint "FK_Chat_OwnerId" references "User"("Id"),
    "ChatUniqueName" varchar(100) unique not null,
    "ChatName" varchar(100) not null,
    "CreatedAt" timestamp not null default CURRENT_TIMESTAMP,
    "LastModified" timestamp not null default CURRENT_TIMESTAMP,
	"IsDeleted" boolean not null default false,
);

create table "Extension"
(
    "Id" smallserial constraint "PK_Extension_Id" primary key,
    "FileCategory" smallint not null,
    "Name" varchar(50) not null,
);

create table "FileMetadata" (
    "Id" bigserial constraint "PK_FileMetadata_Id" primary key,
	"ExtensionId" smallint not null constraint "PK_FileMetadata_ExtensionId" references "Extension"("Id"),
);

CREATE TABLE "ChatUser"
(
    "ChatId" uuid constraint "FK_ChatUser_ChatId" references Chat("Id"),
    "UserId" uuid constraint "FK_ChatUser_UserId" references [User]("Id"),
    "RoleId" int constraint "FK_ChatUser_RoleId" references RoleType("Id"),
	"JoinedAt" timestamp not null default CURRENT_TIMESTAMP,
    constraint "PK_ChatUser_Id" primary key("ChatId", "UserId")
);
CREATE UNIQUE INDEX UX_ChatUser_ChatId_UserId ON ChatUser(ChatId, UserId) WHERE "IsDeleted" = 0
CREATE INDEX IX_ChatUser_UserChat ON ChatUser(UserId, ChatId) WHERE "IsDeleted" = 0;
CREATE INDEX IX_ChatUser_ChatId ON ChatUser(ChatId) WHERE "IsDeleted" = 0;

create table "ChatUserPermission"
(
    "ChatId" int constraint "FK_ChatUserPermission_ChatId" references "ChatUser"("ChatId"),
    "UserId" int constraint "FK_ChatUserPermission_UserId" references "ChatUser"("UserId"),
    "PermissionId" smallint "FK_ChatUserPermission_PermissionId" references "Permission"("Id"),
    constraint "PK_ChatUserPermission_Id" primary key("ChatId", "UserId", "PermissionId")
);

CREATE TABLE StickerPack
(
	"Id" bigserial constraint "PK_StickerPack_Id" primary key,
	"AuthorId" uuid constraint "FK_StickerPack_AuthorId" references "User"("Id"),
	"Title" varchar(100),
	"PreviewId" uuid constraint "FK_StickerPack_PreviewId" references "FileMetadata"("Id"),
	"LastModified" timestamp not null default CURRENT_TIMESTAMP,
	"IsDeleted" boolean not null default false,
);
GO
CREATE INDEX IX_StickerPack_AuthorId ON "StickerPack"("AuthorId") WHERE "IsDeleted" = 0;

CREATE TABLE Sticker
(
	"Id" uuid constraint "PK_Sticker_Id" primary key,
	"StickerPackId" bigint constraint "FK_Sticker_StickerPackId" references "StickerPack"("Id"),
	"FileId" uuid constraint "FK_Sticker_FileId" references "FileMetadata"("Id"),
	"Title" varchar(50),
	"LastModified" timestamp not null default CURRENT_TIMESTAMP,
	"IsDeleted" boolean not null default false,
);

CREATE TABLE UserAvatar
(
	"UserId" uuid constraint "FK_UserAvatar_UserId" references "User"("Id"),
	"FileId" uuid constraint "FK_UserAvatar_FileId" references "FileMetadata"("Id"),
	"IsActive" boolean not null default true,
	"IsDeleted" boolean not null default false,
	"LastModified" timestamp not null default CURRENT_TIMESTAMP,
    constraint "PK_UserAvatar_Id" primary key("UserId", "FileId")
);

CREATE TABLE ChatAvatar
(
	"ChatId" BIGINT constraint "FK_ChatAvatar_ChatId" references "Chat"("Id"),
	"FileId" BIGINT constraint "FK_ChatAvatar_FileId" references "FileMetadata"("Id"),
	"IsActive" boolean not null default true,
	"IsDeleted" boolean not null default false,
	"LastModified" timestamp not null default CURRENT_TIMESTAMP,
    constraint "PK_ChatAvatar_Id" primary key("ChatId", "FileId")
);