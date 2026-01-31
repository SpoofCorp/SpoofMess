create extension "uuid-ossp";

create table "UserEntry"
(
    "Id" uuid primary key,
    "PasswordHash" varchar(100) not null,
    "UniqueName" varchar(100) UNIQUE not null,
    "IsDeleted" boolean not null default false
);

create index "IX_UserEntry_UniqueName" on "UserEntry"("UniqueName");

create table "OperationStatus"
(
	"Id" smallint constraint "PK_OperationStatus_Id" primary key,
	"Name" varchar(50) not null
);

create table "UserEntryOperationStatus"
(
	"Id" bigserial constraint "PK_UserEntryOperationStatus_Id" primary key,
	"UserEntryId" uuid not null constraint "FK_UserEntryOperationStatus_UserEntryId" references "UserEntry"("Id") on delete cascade,
	"OperationStatusId" smallint not null constraint "FK_UserEntryOperationStatus_OperationStatusId" references "OperationStatus"("Id") on delete cascade,
	"Description" text,
	"TimeSet" timestamptz not null default CURRENT_TIMESTAMP,
	"IsActual" boolean not null default true
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

create or replace function "UserEntryOperationStatus_Update_Trigger_Fnc"()
returns trigger as
$$
begin
	update "UserEntryOperationStatus" set "IsActual" = false where "UserEntryId" = new."UserEntryId" and "Id" != new."Id";
    RETURN NEW;
end;
$$ language plpgsql;

create trigger "UserEntryOperationStatus_After_Insert" after insert on "UserEntryOperationStatus"
for each row
execute function "UserEntryOperationStatus_Update_Trigger_Fnc"();

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