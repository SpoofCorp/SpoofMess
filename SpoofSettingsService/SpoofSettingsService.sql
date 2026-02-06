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

create table "GlobalPermission"
(
    "Id" serial constraint "PK_GlobalPermission_Id" primary key,
	"IsDeleted" boolean not null default false,
    "Name" varchar(50) not null unique,
    "Description" varchar(100)
);

create table "Chat"
(
    "Id" uuid constraint "PK_Chat_Id" primary key,
    "ChatTypeId" int not null constraint "FK_Chat_ChatTypeId" references "ChatType"("Id") on delete cascade,
	"OwnerId" uuid constraint "FK_Chat_OwnerId" references "User"("Id") on delete set null,
    "ChatUniqueName" varchar(100) unique not null,
    "ChatName" varchar(100) not null,
    "CreatedAt" timestamp not null default CURRENT_TIMESTAMP,
    "LastModified" timestamp not null default CURRENT_TIMESTAMP,
	"IsDeleted" boolean not null default false
);

create table "RoleRank"
(
    "Id" bigserial constraint "PK_RoleRank_Id" primary key,
    "ChatId" uuid not null constraint "FK_RoleRank_ChatId" references "Chat"("Id") on delete cascade,
	"Level" smallint not null,
	"Name" varchar(50) not null,
	check("Level" between 0 and 255),
	unique("ChatId", "Name")
);

create table "UserGlobalPermission"
(
    "GlobalPermissionId" int not null constraint "FK_UserGlobalPermission_ChatId" references "GlobalPermission"("Id") on delete cascade,
    "UserId" uuid not null constraint "FK_UserGlobalPermission_UserId" references "User"("Id") on delete cascade,
    constraint "PK_UserGlobalPermission_Id" primary key("GlobalPermissionId", "UserId")
);

create table "ChatRole"
(
    "Id" bigserial constraint "PK_ChatRole_Id" primary key,
    "ChatId" uuid not null constraint "FK_ChatRole_ChatId" references "Chat"("Id") on delete cascade,
	"RoleRankId" bigint not null constraint "FK_ChatRole_RoleRankId" references "RoleRank"("Id") on delete cascade,
	"IsDeleted" boolean not null default false,
    "Name" varchar(50) not null,
	constraint "UQ_ChatRole_Chat_Name" unique ("ChatId", "Name")
);

create table "Permission"
(
    "Id" smallint constraint "PK_Permission_Id" primary key,
    "Name" varchar(50) not null,
	"IsDeleted" boolean not null default false,
    "Description" varchar(100)
);

create table "ChatRoleRules"
(
    "ChatRoleId" bigint not null constraint "FK_ChatRoleRules_ChatRoleId" references "ChatRole"("Id") on delete cascade,
    "PermissionId" smallint not null constraint "FK_ChatRoleRules_PermissionId" references "Permission"("Id") on delete cascade,
	"IsPermission" boolean not null default true,
    constraint "PK_RChatRoleRules_Id" primary key("ChatRoleId", "PermissionId")
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
	"JoinedAt" timestamp not null default CURRENT_TIMESTAMP,
	"IsDeleted" boolean not null default false,
    constraint "PK_ChatUser_Id" primary key("ChatId", "UserId")
);
create unique index "UX_ChatUser_ChatId_UserId" on "ChatUser"("ChatId", "UserId") where "IsDeleted" = false;
create index "IX_ChatUser_UserChat" on "ChatUser"("UserId", "ChatId") where "IsDeleted" = false;
create index "IX_ChatUser_ChatId" on "ChatUser"("ChatId") where "IsDeleted" = false;

create table "ChatUserChatRole"
(
    "ChatId" uuid not null,
    "UserId" uuid not null,
    "ChatRoleId" bigint not null constraint "FK_ChatUserChatRole_ChatRoleId" references "ChatRole"("Id"),
	"IsDeleted" boolean not null default false,
	"TimeSet" timestamp not null default CURRENT_TIMESTAMP,
    constraint "FK_ChatUserChatRole_ChatUserId" foreign key("ChatId", "UserId") references "ChatUser"("ChatId", "UserId") on delete cascade,
    constraint "PK_ChatUserChatRole_Id" primary key("ChatId", "UserId", "ChatRoleId")
);

create table "ChatUserRules"
(
    "ChatId" uuid not null,
    "UserId" uuid not null,
	"IsPermission" boolean not null default true,
    "PermissionId" smallint not null constraint "FK_ChatUserPermission_PermissionId" references "Permission"("Id"),
    constraint "FK_ChatUserPermission_ChatUserId" foreign key("ChatId", "UserId") references "ChatUser"("ChatId", "UserId") on delete cascade,
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

insert into "OperationStatus"("Id", "Name")
values 
(0, 'Pending'),
(1, 'Error'),
(2, 'Success'),
(3, 'Rejected'),
(4, 'Deleting');

insert into "Permission"("Id", "Name")
values
(0, 'SendTexts'),
(1, 'SendAudios'),
(2, 'SendVideos'),
(3, 'SendFiles'),
(4, 'Muting'),
(5, 'Banning'),
(6, 'Sharing'),
(7, 'Inviting'),
(8, 'Deleting'),
(9, 'Editing');

create or replace function "FileMetadataOperationStatus_OnceActual"()
returns trigger as
$$
begin
	if NOT new."IsActive" THEN
		return new;
	end if;
	update "FileMetadataOperationStatus" set "IsActual" = false where "FileMetadataId" = new."FileMetadataId" and "IsActual" = true and "Id" != new."Id";
	return new;
end;
$$ language plpgsql;


create trigger "Trg_FileMetadataOperationStatus_After_Insert" after insert on "FileMetadataOperationStatus"
for each row
execute function "FileMetadataOperationStatus_OnceActual"();

create or replace function "CreateRolesAfterChat"()
returns trigger as
$$
declare
topRankId bigint;
middleRankId bigint;
leastRankId bigint;
ownerRoleId bigint;
adminRoleId bigint;
memberRoleId bigint;
begin
	if new."IsDeleted" THEN
		return new;
	end if;
	
	insert into "RoleRank"("ChatId", "Level", "Name")
	values (new."Id", 0, 'Top')
	returning "Id" into topRankId; 

	insert into "RoleRank"("ChatId", "Level", "Name")
	values (new."Id", 2, 'Middle')
	returning "Id" into middleRankId; 

	insert into "RoleRank"("ChatId", "Level", "Name")
	values (new."Id", 5, 'Least')
	returning "Id" into leastRankId; 

	insert into "ChatRole"("ChatId", "Name", "RoleRankId")
	values (new."Id", 'Owner', topRankId)
	returning "Id" into ownerRoleId;

	insert into "ChatRole"("ChatId", "Name", "RoleRankId")
	values (new."Id", 'Admin', middleRankId)
	returning "Id" into adminRoleId;

	insert into "ChatRole"("ChatId", "Name", "RoleRankId")
	values (new."Id", 'Member', leastRankId)
	returning "Id" into memberRoleId;

	insert into "ChatUser"("ChatId", "UserId")
	values
	(new."Id", new."OwnerId");

	insert into "ChatUserChatRole"("ChatId", "UserId", "ChatRoleId")
	values
	(new."Id", new."OwnerId", ownerRoleId);
	
	insert into "ChatRoleRules"("ChatRoleId", "PermissionId", "IsPermission")
	values
	(memberRoleId, 0, true),
	(memberRoleId, 1, true),
	(memberRoleId, 2, true),
	(memberRoleId, 3, true),
	(memberRoleId, 6, true),
	(memberRoleId, 7, true),
	(memberRoleId, 8, true),
	(memberRoleId, 9, true),
	(adminRoleId, 0, true),
	(adminRoleId, 1, true),
	(adminRoleId, 2, true),
	(adminRoleId, 3, true),
	(adminRoleId, 6, true),
	(adminRoleId, 7, true),
	(adminRoleId, 8, true),
	(adminRoleId, 9, true),
	(adminRoleId, 4, true),
	(adminRoleId, 5, true);

	insert into "ChatRoleRules"("ChatRoleId", "PermissionId", "IsPermission")
	select ownerRoleId, "Id", true from "Permission";
	
	return new;
end;
$$ language plpgsql;

create trigger "Trg_Chat_After_Insert" after insert on "Chat"
for each row
execute function "CreateRolesAfterChat"();

create or replace function check_user_permission(
	p_user_id uuid,
	p_chat_id uuid,
	p_permission_id smallint
)
returns int as
$$
declare
isPermission bool;
begin
	select "IsPermission" into isPermission
	from "ChatUserRules" where "ChatId" = p_chat_id and "UserId" = p_user_id and "PermissionId" = p_permission_id;
	if found then
		return case when isPermission then 1 else 2 end;
	end if;

	select crr."IsPermission" into isPermission
	from "ChatUserChatRole" cucr
	join "ChatRole" cr on cr."Id" = cucr."ChatRoleId" 
	join "RoleRank" rr on rr."Id" = cr."RoleRankId"
	join "ChatRoleRules" crr on crr."ChatRoleId" = cucr."ChatRoleId" and crr."PermissionId" = p_permission_id
	where cucr."ChatId" = p_chat_id and cucr."UserId" = p_user_id
	order by rr."Level" asc
	limit 1;

	if found then
		return case when isPermission then 1 else 2 end;
	end if;
	return 0;
end;
$$ language plpgsql;