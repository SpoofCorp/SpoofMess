create table "User"
(
	"Id" uuid constraint "PK_User_Id" primary key default uuidv7(),
	"WasOnline" timestamp not null default CURRENT_TIMESTAMP,
	"Name" varchar(100) not null,
	"MonthsBeforeDelete" int not null default 6,
	"SearchMe" boolean not null default true,
	"ShowMe" boolean not null default true,
	"ForwardMessage" boolean not null default true,
	"InviteMe" boolean not null default true,
	"IsDeleted" boolean not null default false
);

create table "ChatType"
(
    "Id" serial constraint "PK_ChatType_Id" primary key,
	"IsDeleted" boolean not null default false,
    "Name" varchar(50) not null unique
);

create table "ChatProperty"
(
    "Id" smallserial constraint "PK_ChatProperty_Id" primary key,
    "Name" varchar(50) not null,
	"IsDeleted" boolean not null default false,
    "Description" varchar(100)
);

create table "ChatTypeChatProperty"
(
    "ChatTypeId" int not null constraint "FK_ChatTypeChatProperty_ChatTypeId" references "ChatType"("Id") on delete cascade,
    "ChatPropertyId" smallint not null constraint "FK_ChatTypeChatProperty_ChatPropertyId" references "ChatProperty"("Id") on delete cascade,
	"IsDeleted" boolean not null default false,
    constraint "PK_ChatTypeChatProperty_Id" primary key("ChatTypeId", "ChatPropertyId")
);

create table "Role"
(
    "Id" serial constraint "PK_Role_Id" primary key,
	"IsDeleted" boolean not null default false,
    "Name" varchar(50) not null unique
);

create table "Permission"
(
    "Id" smallserial constraint "PK_Permission_Id" primary key,
    "Name" varchar(50) not null,
	"IsDeleted" boolean not null default false,
    "Description" varchar(100)
);

create table "RolePermission"
(
    "RoleId" int not null constraint "FK_RolePermission_RoleId" references "Role"("Id") on delete cascade,
    "PermissionId" smallint not null constraint "FK_RolePermission_PermissionId" references "Permission"("Id") on delete cascade,
	"IsDeleted" boolean not null default false,
    constraint "PK_RolePermission_Id" primary key("RoleId", "PermissionId")
);

create table "Chat"
(
    "Id" uuid constraint "PK_Chat_Id" primary key,
    "ChatTypeId" int not null not null constraint "FK_Chat_ChatTypeId" references "ChatType"("Id") on delete cascade,
	"OwnerId" uuid constraint "FK_Chat_OwnerId" references "User"("Id") on delete cascade,
    "ChatUniqueName" varchar(100) unique not null,
    "ChatName" varchar(100) not null,
    "CreatedAt" timestamp not null default CURRENT_TIMESTAMP,
    "LastModified" timestamp not null default CURRENT_TIMESTAMP,
	"IsDeleted" boolean not null default false
);

create table "Extension"
(
    "Id" smallserial constraint "PK_Extension_Id" primary key,
    "FileCategory" smallint not null,
	"IsDeleted" boolean not null default false,
    "Name" varchar(50) not null
);

create table "FileMetadata" (
    "Id" uuid constraint "PK_FileMetadata_Id" primary key,
	"IsDeleted" boolean not null default false,
    "Size" bigint not null,
	"ExtensionId" smallint not null constraint "PK_FileMetadata_ExtensionId" references "Extension"("Id") on delete cascade
);

create table "OperationStatus"
(
	"Id" smallint constraint "PK_OperationStatus_Id" primary key,
	"Name" varchar(50) not null
);

create table "FileMetadataOperationStatus"
(
	"Id" bigserial constraint "PK_FileMetadataOperationStatus_Id" primary key,
	"FileMetadataId" uuid not null constraint "FK_FileMetadataOperationStatus_MessageId" references "FileMetadata"("Id") on delete cascade,
	"OperationStatusId" smallint not null constraint "FK_FileMetadataOperationStatus_OperationStatusId" references "OperationStatus"("Id") on delete cascade,
	"Description" text,
	"TimeSet" timestamp not null default CURRENT_TIMESTAMP,
	"IsActual" boolean not null default true
);

create TABLE "ChatUser"
(
    "ChatId" uuid not null constraint "FK_ChatUser_ChatId" references "Chat"("Id") on delete cascade,
    "UserId" uuid not null constraint "FK_ChatUser_UserId" references "User"("Id") on delete cascade,
    "RoleId" int not null constraint "FK_ChatUser_RoleId" references "Role"("Id"),
	"JoinedAt" timestamp not null default CURRENT_TIMESTAMP,
	"IsDeleted" boolean not null default false,
    constraint "PK_ChatUser_Id" primary key("ChatId", "UserId")
);
create unique index "UX_ChatUser_ChatId_UserId" on "ChatUser"("ChatId", "UserId") where "IsDeleted" = false;
create index "IX_ChatUser_UserChat" on "ChatUser"("UserId", "ChatId") where "IsDeleted" = false;
create index "IX_ChatUser_ChatId" on "ChatUser"("ChatId") where "IsDeleted" = false;

create table "ChatUserPermission"
(
    "ChatId" uuid not null,
    "UserId" uuid not null,
    "PermissionId" smallint not null constraint "FK_ChatUserPermission_PermissionId" references "Permission"("Id"),
	"IsDeleted" boolean not null default false,
    constraint "FK_ChatUserPermission_ChatUser" foreign key("ChatId", "UserId") references "ChatUser"("ChatId", "UserId") on delete cascade,
    constraint "PK_ChatUserPermission_Id" primary key("ChatId", "UserId", "PermissionId")
);

create table "StickerPack"
(
	"Id" bigserial constraint "PK_StickerPack_Id" primary key,
	"AuthorId" uuid not null constraint "FK_StickerPack_AuthorId" references "User"("Id") on delete cascade,
	"PreviewId" uuid not null constraint "FK_StickerPack_PreviewId" references "FileMetadata"("Id") on delete cascade,
	"Title" varchar(100),
	"LastModified" timestamp not null default CURRENT_TIMESTAMP,
	"IsDeleted" boolean not null default false
);

create index "IX_StickerPack_AuthorId" on "StickerPack"("AuthorId") where "IsDeleted" = false;

create table "Sticker"
(
	"Id" uuid constraint "PK_Sticker_Id" primary key default uuidv7(),
	"StickerPackId" bigint not null constraint "FK_Sticker_StickerPackId" references "StickerPack"("Id") on delete cascade,
	"FileId" uuid not null constraint "FK_Sticker_FileId" references "FileMetadata"("Id") on delete cascade,
	"Title" varchar(50) not null,
	"LastModified" timestamp not null default CURRENT_TIMESTAMP,
	"IsDeleted" boolean not null default false
);

create table "UserAvatar"
(
	"UserId" uuid not null constraint "FK_UserAvatar_UserId" references "User"("Id") on delete cascade,
	"FileId" uuid not null constraint "FK_UserAvatar_FileId" references "FileMetadata"("Id") on delete cascade,
	"IsActive" boolean not null default true,
	"IsDeleted" boolean not null default false,
	"LastModified" timestamp not null default CURRENT_TIMESTAMP,
    constraint "PK_UserAvatar_Id" primary key("UserId", "FileId")
);

create table "ChatAvatar"
(
	"ChatId" uuid not null constraint "FK_ChatAvatar_ChatId" references "Chat"("Id") on delete cascade,
	"FileId" uuid not null constraint "FK_ChatAvatar_FileId" references "FileMetadata"("Id") on delete cascade,
	"IsActive" boolean not null default true,
	"IsDeleted" boolean not null default false,
	"LastModified" timestamp not null default CURRENT_TIMESTAMP,
    constraint "PK_ChatAvatar_Id" primary key("ChatId", "FileId")
);