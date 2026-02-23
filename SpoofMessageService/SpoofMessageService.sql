create table "OperationStatus"
(
	"Id" smallint constraint "PK_OperationStatus_Id" primary key,
	"Name" varchar(50) not null
);

create table "User"
(
	"Id" uuid constraint "PK_User_Id" primary key default uuidv7(),
	"AvatarId" uuid,
	"Login" varchar(100) unique not null,
	"Name" varchar(100) not null,
	"LastModified" timestamp not null default CURRENT_TIMESTAMP,
	"IsDeleted" boolean not null default false
);

create table "Chat"
(
	"Id" uuid constraint "PK_Chat_Id" primary key default uuidv7(),
	"AvatarId" uuid,
	"UniqueName" varchar(100) unique not null,
	"Name" varchar(100),
	"LastModified" timestamp not null default CURRENT_TIMESTAMP,
	"IsDeleted" boolean not null default false
);

create TABLE "ChatUser"
(
    "ChatId" uuid not null constraint "FK_ChatUser_ChatId" references "Chat"("Id") on delete cascade,
    "UserId" uuid not null constraint "FK_ChatUser_UserId" references "User"("Id") on delete cascade,
	"Rules" bigint default 0 not null,
	"JoinedAt" timestamptz not null default CURRENT_TIMESTAMP,
	"IsDeleted" boolean not null default false,
    constraint "PK_ChatUser_Id" primary key("ChatId", "UserId")
);

create table "Message"
(
	"Id" uuid constraint "PK_Message_Id" primary key default uuidv7(),
	"Text" text not null default '',
	"ChatId" uuid not null constraint "FK_Message_ChatId" references "Chat"("Id") on delete cascade,
	"UserId" uuid not null constraint "FK_Message_UserId" references "User"("Id") on delete cascade,
	"CountViews" int not null default 0,
	"SentAt" timestamp not null default CURRENT_TIMESTAMP,
	"LastModified" timestamp not null default CURRENT_TIMESTAMP,
	"IsDeleted" boolean not null default false
);

create table "MessageOperationStatus"
(
	"Id" bigserial constraint "PK_MessageOperationStatus_Id" primary key,
	"MessageId" uuid not null constraint "FK_MessageOperationStatus_MessageId" references "Message"("Id") on delete cascade,
	"OperationStatusId" smallint not null constraint "FK_MessageOperationStatus_OperationStatusId" references "OperationStatus"("Id") on delete cascade,
	"Description" text,
	"TimeSet" timestamp not null default CURRENT_TIMESTAMP,
	"IsActual" boolean not null default true
);

create table "FileType"
(
    "Id" smallserial constraint "PK_FileType_Id" primary key,
	"IsDeleted" boolean not null default false,
    "Name" varchar(50) not null
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
    "Size" bigint not null,
	"IsDeleted" boolean not null default false,
	"ExtensionId" smallint not null constraint "PK_FileMetadata_ExtensionId" references "Extension"("Id") on delete cascade
);

create index "IX_Message_LastMessages" on "Message"("ChatId", "SentAt") include ("Text");
create index "IX_Message_ChatId" on "Message"("ChatId");
create index "IX_Message_AuthorId" on "Message"("UserId");
create index "IX_Message_SentAt" on "Message"("SentAt");

create table "Attachment"
(
	"MessageId"	uuid not null constraint "FK_Attachment_MessageId" references "Message"("Id") on delete cascade, 
	"FileMetadataId" uuid not null constraint "FK_Attachment_FileMetadataId" references "FileMetadata"("Id") on delete cascade,
	"IsDeleted" boolean not null default false,
	"LastModified" timestamp not null default CURRENT_TIMESTAMP,
	constraint "PK_Attachment_Id" primary key("MessageId", "FileMetadataId")
);

create table "AttachmentOperationStatus"
(
	"Id" bigserial constraint "PK_AttachmentOperationStatus_Id" primary key,
	"MessageId"	uuid not null, 
	"FileMetadataId" uuid not null,
	"OperationStatusId" smallint not null constraint "FK_AttachmentOperationStatus_OperationStatusId" references "OperationStatus"("Id") on delete cascade,
	"Description" text,
	"TimeSet" timestamp not null default CURRENT_TIMESTAMP,
	"IsActual" boolean not null default true,
	constraint "FK_AttachmentOperationStatus_AttachmentId" foreign key ("MessageId", "FileMetadataId") references "Attachment"("MessageId", "FileMetadataId") on delete cascade
);

create table "ViewMessage"
(
	"UserId" uuid not null constraint "FK_ViewMessage_UserId" references "User"("Id") on delete cascade,
	"MessageId"	uuid not null constraint "FK_ViewMessage_MessageId" references "Message"("Id") on delete cascade, 
	"ViewTime" timestamp not null default CURRENT_TIMESTAMP,
	"IsDeleted" boolean not null default false,
	"LastModified" timestamp not null default CURRENT_TIMESTAMP,
	constraint "PK_ViewMessage_Id" primary key("MessageId", "UserId")
);

create index "IX_ViewMessage_MessageId" on "ViewMessage"("MessageId");
create index "IX_ViewMessage_UserId" on "ViewMessage"("UserId");


insert into "OperationStatus"("Id", "Name")
values 
(0, 'Pending'),
(1, 'Error'),
(2, 'Success'),
(3, 'Rejected'),
(4, 'Deleting');

create or replace function "Fnc_ViewMessage_Count"()
returns trigger as
$$
begin
	update "Message" set "CountViews" = "CountViews" + 1 where "Id" = new."MessageId";
	
	return new;
end;
$$ language plpgsql;

create trigger "Trg_ViewMessage_After_Insert" after insert on "ViewMessage"
for each row
execute function "Fnc_ViewMessage_Count"();