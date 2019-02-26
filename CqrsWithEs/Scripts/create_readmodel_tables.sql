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


create table policy_version_view
(
policy_version_id uuid not null primary key,
policy_id uuid not null,
policy_number character varying(50) not null,
product_code character varying(50) not null,
version_number int not null,
version_status character varying(50) not null,
policy_status character varying(50) not null,
policy_holder character varying(250) not null,
insured character varying(250) not null,
car character varying(250) not null,
cover_from timestamp with time zone not null,
cover_to timestamp with time zone not null,
version_from timestamp with time zone not null,
version_to timestamp with time zone not null,
total_premium numeric(19,2) not null
);

create table policy_version_cover
(
policy_version_cover_id  uuid not null primary key,
policy_version_id uuid not null,
code character varying(50) not null,
cover_from timestamp with time zone not null,
cover_to timestamp with time zone not null,
premium_amount numeric(19,2) not null,
constraint policy_version_fk foreign key (policy_version_id) references policy_version_view(policy_version_id)
);