create table "OperationStatus"
(
	"Id" smallint constraint "PK_OperationStatus_Id" primary key,
	"Name" varchar(50) not null
);

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
	"UserId" uuid not null, 
	"MessageId"	uuid not null constraint "FK_ViewMessage_MessageId" references "Message"("Id") on delete cascade, 
	"ViewTime" timestamp not null default CURRENT_TIMESTAMP,
	"IsDeleted" boolean not null default false,
	"LastModified" timestamp not null default CURRENT_TIMESTAMP,
	constraint "PK_ViewMessage_Id" primary key("MessageId", "UserId")
);

create index "IX_ViewMessage_MessageId" on "ViewMessage"("MessageId");
create index "IX_ViewMessage_UserId" on "ViewMessage"("UserId");
