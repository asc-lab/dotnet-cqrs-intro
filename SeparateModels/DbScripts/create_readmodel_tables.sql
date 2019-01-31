create table  policy_info_view
(
  policy_id uuid not null primary key,
  policy_number character varying(50) not null,
  cover_from timestamp with time zone not null,
  cover_to timestamp with time zone not null,
  vehicle character varying(150) not null,
  policy_holder character varying(50) not null,
  total_premium numeric(19,2) not null
);