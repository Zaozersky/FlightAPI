PGDMP                         |         
   AirportDB2    15.4    15.4                0    0    ENCODING    ENCODING        SET client_encoding = 'UTF8';
                      false                       0    0 
   STDSTRINGS 
   STDSTRINGS     (   SET standard_conforming_strings = 'on';
                      false                       0    0 
   SEARCHPATH 
   SEARCHPATH     8   SELECT pg_catalog.set_config('search_path', '', false);
                      false                       1262    16481 
   AirportDB2    DATABASE     n   CREATE DATABASE "AirportDB2" WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE_PROVIDER = libc LOCALE = 'C';
    DROP DATABASE "AirportDB2";
                postgres    false            �            1259    16482    AviasalesFlights    TABLE     e  CREATE TABLE public."AviasalesFlights" (
    id integer NOT NULL,
    airport_from character varying(256),
    airport_to character varying(256),
    airline character varying(256),
    departure_date date,
    arrival_date date,
    transfers_count integer,
    price numeric,
    currency character varying(10),
    flight_number character varying(10)
);
 &   DROP TABLE public."AviasalesFlights";
       public         heap    postgres    false            �            1259    16487    AviasalesOrders    TABLE     Z   CREATE TABLE public."AviasalesOrders" (
    id integer NOT NULL,
    flight_id integer
);
 %   DROP TABLE public."AviasalesOrders";
       public         heap    postgres    false            �            1259    16490    Orders_id_seq    SEQUENCE     �   CREATE SEQUENCE public."Orders_id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 &   DROP SEQUENCE public."Orders_id_seq";
       public          postgres    false    215                       0    0    Orders_id_seq    SEQUENCE OWNED BY     L   ALTER SEQUENCE public."Orders_id_seq" OWNED BY public."AviasalesOrders".id;
          public          postgres    false    216            �            1259    16491    flights_id_seq    SEQUENCE     �   CREATE SEQUENCE public.flights_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 %   DROP SEQUENCE public.flights_id_seq;
       public          postgres    false    214                       0    0    flights_id_seq    SEQUENCE OWNED BY     L   ALTER SEQUENCE public.flights_id_seq OWNED BY public."AviasalesFlights".id;
          public          postgres    false    217            t           2604    16492    AviasalesFlights id    DEFAULT     s   ALTER TABLE ONLY public."AviasalesFlights" ALTER COLUMN id SET DEFAULT nextval('public.flights_id_seq'::regclass);
 D   ALTER TABLE public."AviasalesFlights" ALTER COLUMN id DROP DEFAULT;
       public          postgres    false    217    214            u           2604    16493    AviasalesOrders id    DEFAULT     s   ALTER TABLE ONLY public."AviasalesOrders" ALTER COLUMN id SET DEFAULT nextval('public."Orders_id_seq"'::regclass);
 C   ALTER TABLE public."AviasalesOrders" ALTER COLUMN id DROP DEFAULT;
       public          postgres    false    216    215                      0    16482    AviasalesFlights 
   TABLE DATA           �   COPY public."AviasalesFlights" (id, airport_from, airport_to, airline, departure_date, arrival_date, transfers_count, price, currency, flight_number) FROM stdin;
    public          postgres    false    214   �                 0    16487    AviasalesOrders 
   TABLE DATA           :   COPY public."AviasalesOrders" (id, flight_id) FROM stdin;
    public          postgres    false    215   >                  0    0    Orders_id_seq    SEQUENCE SET     =   SELECT pg_catalog.setval('public."Orders_id_seq"', 5, true);
          public          postgres    false    216                       0    0    flights_id_seq    SEQUENCE SET     <   SELECT pg_catalog.setval('public.flights_id_seq', 3, true);
          public          postgres    false    217            w           2606    16495    AviasalesFlights flights_pkey 
   CONSTRAINT     ]   ALTER TABLE ONLY public."AviasalesFlights"
    ADD CONSTRAINT flights_pkey PRIMARY KEY (id);
 I   ALTER TABLE ONLY public."AviasalesFlights" DROP CONSTRAINT flights_pkey;
       public            postgres    false    214            x           2606    16496 %   AviasalesOrders Orders_flight_id_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."AviasalesOrders"
    ADD CONSTRAINT "Orders_flight_id_fkey" FOREIGN KEY (flight_id) REFERENCES public."AviasalesFlights"(id);
 S   ALTER TABLE ONLY public."AviasalesOrders" DROP CONSTRAINT "Orders_flight_id_fkey";
       public          postgres    false    214    215    3447               X   x�-�;� E���^0� 0�*$� ������O�nq�C�j-��66ў�H��ʄv%��ybs²�.ܿiO��_�ۉ�Q���}&            x������ � �     