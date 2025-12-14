-- ===========================================
-- Enable UUID generator
-- ===========================================
CREATE EXTENSION IF NOT EXISTS "pgcrypto";

-- ===========================================
-- Users Table
-- ===========================================
CREATE TABLE "Users" (
    "Id" UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    "Name" TEXT NOT NULL,
    "Email" TEXT NOT NULL,
    "Password" TEXT NOT NULL,
    "PhoneNumber" TEXT
);

-- ===========================================
-- Message Table
-- ===========================================
create table public."Message" (
  "Id" uuid not null default gen_random_uuid (),
  "UserId" uuid not null,
  "Sender" text not null,
  "MessageContent" text not null,
  "Time" timestamp with time zone null default (now() AT TIME ZONE 'utc'::text),
  constraint Message_pkey primary key ("Id"),
  constraint Message_UserId_fkey foreign KEY ("UserId") references "Users" ("Id") on delete CASCADE
) TABLESPACE pg_default;

-- ===========================================
-- Payment Table
-- ===========================================
CREATE TABLE "Payment" (
  "Id" uuid not null default gen_random_uuid (),
  "UserId" uuid not null,
  "MessageId" uuid not null,
  constraint Payment_pkey primary key ("Id"),
  constraint Payment_MessageId_fkey foreign KEY ("MessageId") references "Message" ("Id") on delete CASCADE,
  constraint Payment_UserId_fkey foreign KEY ("UserId") references "Users" ("Id") on delete CASCADE
);

-- ===========================================
-- Transaction Table
-- ===========================================
CREATE TABLE "Transaction" (
  "Id" uuid not null default gen_random_uuid (),
  "Type" text not null,
  "Amount" numeric(10, 2) not null,
  "Target" text null,
  "Time" timestamp with time zone not null,
  "MessageId" uuid not null,
  "Account" text null,
  "UserId" uuid null,
  constraint Transaction_pkey primary key ("Id"),
  constraint Transaction_MessageId_fkey foreign KEY ("MessageId") references "Message" ("Id"),
  constraint Transaction_UserId_fkey foreign KEY ("UserId") references "Users" ("Id") on delete CASCADE
);
