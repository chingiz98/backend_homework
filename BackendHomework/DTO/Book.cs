using System;

namespace BackendHomework.Models
{
    /*
 CREATE TABLE "books" (
 "id" uuid NOT NULL DEFAULT uuid_generate_v4(),
 "name" varchar(255),
 "url" varchar(255)
);
     */
    public class Book
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }
}
