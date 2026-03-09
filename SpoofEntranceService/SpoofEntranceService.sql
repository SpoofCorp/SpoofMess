create table "UserEntry"
(
    "Id" uuid primary key,
    "PasswordHash" varchar(100) not null,
    "UniqueName" varchar(100) unique not null,
    "IsDeleted" boolean not null default false
);

create index "IX_UserEntry_UniqueName" on "UserEntry"("UniqueName");

create type "OutboxStatus" as enum 
(
    'Pending',
    'Error', 
    'Success',
    'Rejected',
    'Deleting'
);

create table "UserEntryOutbox"
(
	"Id" uuid constraint "PK_UserEntryOutbox_Id" primary key default uuidv7(),
    "UserEntryId" uuid not null constraint "FK_UserEntryOutbox_UserEntryId" references "UserEntry"("Id") on delete cascade,
	"IsSynced" boolean not null default false,
	"LastTryDate" timestamptz not null default CURRENT_TIMESTAMP,
	"CreatedAt" timestamptz not null default CURRENT_TIMESTAMP,
	"Data" jsonb not null,
	"Status" "OutboxStatus" not null
);

create table "SessionInfo"
(
    "Id" uuid primary key,
    "UserEntryId" uuid not null,
    "DeviceId" varchar(100) not null,
    "DeviceName" varchar(255),
    "Platform" varchar(50),
    "UserAgent" varchar(500),
    "IpAddress" varchar(45),
    "CreatedAt" timestamptz not null default CURRENT_TIMESTAMP,
    "LastActivityAt" timestamptz not null default CURRENT_TIMESTAMP,
    "IsActive" boolean not null default TRUE,
    "IsDeleted" boolean not null default false,
    
    CONSTRAINT "FK_SessionInfo_UserEntryId" 
        FOREIGN key ("UserEntryId") 
        REFERENCES "UserEntry"("Id") 
        on DELETE CASCADE
);

create index "IX_SessionInfo_Id" on "SessionInfo"("Id");
create index "IX_SessionInfo_UserId_Active" 
    on "SessionInfo"("UserEntryId") 
    where "IsActive" = true and "IsDeleted" = false;

create table "Token"
(
    "RefreshTokenHash" varchar(100) primary key,
    "SessionInfoId" uuid not null,
    "ValidTo" timestamptz not null,
    "IsDeleted" boolean not null default false,
    
    constraint "FK_Token_SessionInfoId" 
        foreign key ("SessionInfoId") 
        references "SessionInfo"("Id") 
        on delete cascade
);

create index "IX_Token_SessionInfoId" 
    on "Token"("SessionInfoId") 
    where "IsDeleted" = false;
    
create index "IX_Token_ValidTo" 
    on "Token"("RefreshTokenHash") 
    where "IsDeleted" = false;

create or replace function "TokenOnceInsert_Trigger_Fnc"()
returns trigger as
$$
begin
	update "Token" set "IsDeleted" = true where "SessionInfoId" = new."SessionInfoId" and "RefreshTokenHash" != new."RefreshTokenHash";
	return new;
end;
$$ language plpgsql;

create trigger "Token_After_Insert" after insert on "Token"
for each row
execute function "TokenOnceInsert_Trigger_Fnc"();