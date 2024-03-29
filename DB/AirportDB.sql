PGDMP                         |         	   AirportDB    15.4    15.4                0    0    ENCODING    ENCODING        SET client_encoding = 'UTF8';
                      false                       0    0 
   STDSTRINGS 
   STDSTRINGS     (   SET standard_conforming_strings = 'on';
                      false                       0    0 
   SEARCHPATH 
   SEARCHPATH     8   SELECT pg_catalog.set_config('search_path', '', false);
                      false                       1262    16436 	   AirportDB    DATABASE     m   CREATE DATABASE "AirportDB" WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE_PROVIDER = libc LOCALE = 'C';
    DROP DATABASE "AirportDB";
                postgres    false            �            1259    16438    Flights    TABLE     G  CREATE TABLE public."Flights" (
    id integer NOT NULL,
    origin character varying(256),
    destination character varying(256),
    airline character varying(256),
    departure date,
    arrival date,
    transfers integer,
    price numeric,
    currency character varying(10),
    flight_number character varying(10)
);
    DROP TABLE public."Flights";
       public         heap    postgres    false            �            1259    16472    Orders    TABLE     Q   CREATE TABLE public."Orders" (
    id integer NOT NULL,
    flight_id integer
);
    DROP TABLE public."Orders";
       public         heap    postgres    false            �            1259    16471    Orders_id_seq    SEQUENCE     �   CREATE SEQUENCE public."Orders_id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 &   DROP SEQUENCE public."Orders_id_seq";
       public          postgres    false    217                       0    0    Orders_id_seq    SEQUENCE OWNED BY     C   ALTER SEQUENCE public."Orders_id_seq" OWNED BY public."Orders".id;
          public          postgres    false    216            �            1259    16437    flights_id_seq    SEQUENCE     �   CREATE SEQUENCE public.flights_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 %   DROP SEQUENCE public.flights_id_seq;
       public          postgres    false    215                       0    0    flights_id_seq    SEQUENCE OWNED BY     C   ALTER SEQUENCE public.flights_id_seq OWNED BY public."Flights".id;
          public          postgres    false    214            t           2604    16441 
   Flights id    DEFAULT     j   ALTER TABLE ONLY public."Flights" ALTER COLUMN id SET DEFAULT nextval('public.flights_id_seq'::regclass);
 ;   ALTER TABLE public."Flights" ALTER COLUMN id DROP DEFAULT;
       public          postgres    false    215    214    215            u           2604    16475 	   Orders id    DEFAULT     j   ALTER TABLE ONLY public."Orders" ALTER COLUMN id SET DEFAULT nextval('public."Orders_id_seq"'::regclass);
 :   ALTER TABLE public."Orders" ALTER COLUMN id DROP DEFAULT;
       public          postgres    false    216    217    217                      0    16438    Flights 
   TABLE DATA           �   COPY public."Flights" (id, origin, destination, airline, departure, arrival, transfers, price, currency, flight_number) FROM stdin;
    public          postgres    false    215   �       
          0    16472    Orders 
   TABLE DATA           1   COPY public."Orders" (id, flight_id) FROM stdin;
    public          postgres    false    217   ~                  0    0    Orders_id_seq    SEQUENCE SET     >   SELECT pg_catalog.setval('public."Orders_id_seq"', 1, false);
          public          postgres    false    216                       0    0    flights_id_seq    SEQUENCE SET     <   SELECT pg_catalog.setval('public.flights_id_seq', 4, true);
          public          postgres    false    214            w           2606    16445    Flights flights_pkey 
   CONSTRAINT     T   ALTER TABLE ONLY public."Flights"
    ADD CONSTRAINT flights_pkey PRIMARY KEY (id);
 @   ALTER TABLE ONLY public."Flights" DROP CONSTRAINT flights_pkey;
       public            postgres    false    215            x           2606    16476    Orders Orders_flight_id_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."Orders"
    ADD CONSTRAINT "Orders_flight_id_fkey" FOREIGN KEY (flight_id) REFERENCES public."Flights"(id);
 J   ALTER TABLE ONLY public."Orders" DROP CONSTRAINT "Orders_flight_id_fkey";
       public          postgres    false    217    3447    215               �   x�=���0E�ۯ�j^[�e!Q6B��EMKb�{�s�srڢBy�p)!I*N�K�p�� d���AI���gt]�)�����0���+$�JY�vx���������ϸZ��w�Ӷ?�֚М�_8b�^�=��6�qd�}	),h      
      x������ � �     