create extension "uuid-ossp";

create table "UserEntry"
(
    "Id" uuid primary key,
    "PasswordHash" varchar(100) not null,
    "UniqueName" varchar(100) UNIQUE not null,
    "IsDeleted" boolean not null default false
);

create index "IX_UserEntry_UniqueName" on "UserEntry"("UniqueName");

create table "SessionInfo"
(
    "Id" uuid primary key,
    "UserEntryId" uuid not null,
    "DeviceId" varchar(100) not null,
    "DeviceName" varchar(255),
    "Platform" varchar(50),
    "UserAgent" varchar(500),
    "IpAddress" varchar(45),
    "CreatedAt" timestamp not null default CURRENT_TIMESTAMP,
    "LastActivityAt" timestamp not null default CURRENT_TIMESTAMP,
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
    "ValidTo" timestamp not null,
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