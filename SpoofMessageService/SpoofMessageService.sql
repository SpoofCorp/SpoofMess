create table "Message"
(
	"Id" uuid constraint "PK_Message_Id" primary key default uuidv7(),
	"Text" text not null default '',
	"ChatId" uuid not null,
	"UserId" uuid not null,
	"SentAt" timestamp not null default CURRENT_TIMESTAMP,
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

create table "ViewMessage"
(
	"UserId" uuid not null, 
	"MessageId"	uuid not null constraint "FK_ViewMessage_MessageId" references "Message"("Id") on delete cascade, 
	"ViewTime" timestamp not null default CURRENT_TIMESTAMP,
	"IsDeleted" boolean not null default false,
	"LastModified" timestamp not null default CURRENT_TIMESTAMP,
	constraint "PK_ViewMessage_Id" primary key("MessageId", "UserId")

);

create index "IX_ViewMessage_MessageId" on "ViewMessage"("MessageId");
create index "IX_ViewMessage_UserId" on "ViewMessage"("UserId");
