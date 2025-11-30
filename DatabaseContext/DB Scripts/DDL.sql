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
    "Email" TEXT NOT NULL UNIQUE,
    "Password" TEXT NOT NULL,
    "PhoneNumber" TEXT
);

-- ===========================================
-- Message Table
-- ===========================================
create table public."Message" (
  "Id" uuid not null default gen_random_uuid (),
  "UserId" uuid not null,
  "MessageContent" text not null,
  "Time" timestamp with time zone null default (now() AT TIME ZONE 'utc'::text),
  constraint Message_pkey primary key ("Id"),
  constraint Message_UserId_fkey foreign KEY ("UserId") references "Users" ("Id")
) TABLESPACE pg_default;

-- ===========================================
-- Payment Table
-- ===========================================
CREATE TABLE "Payment" (
    "Id" UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    "UserId" UUID NOT NULL REFERENCES "Users"("Id"),
    "MessageId" UUID NOT NULL REFERENCES "Message"("Id")
);

-- ===========================================
-- Transaction Table
-- ===========================================
CREATE TABLE "Transaction" (
    "Id" UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    "Type" TEXT NOT NULL,
    "Amount" NUMERIC(10,2) NOT NULL,
    "Account" TEXT,
    "Target" TEXT,
    "Time" TIMESTAMPTZ NOT NULL,
    "MessageId" UUID NOT NULL REFERENCES "Message"("Id")
);
