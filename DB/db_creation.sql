CREATE SCHEMA `fashion_forward_db` DEFAULT CHARACTER SET utf8 ;

-- USER TABLE

CREATE TABLE `fashion_forward_db`.`user` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `email` VARCHAR(255) NULL,
  `password` VARCHAR(255) NULL,
  `active` INT NOT NULL,
  `verify` INT NOT NULL,
  PRIMARY KEY (`id`));

-- PRODUCT TABLE

CREATE TABLE product (
  `id` INT AUTO_INCREMENT PRIMARY KEY,
  `code` VARCHAR(255),
  `name` VARCHAR(255),
  `price` DECIMAL(10, 2),
  `category` VARCHAR(255),
  `image` VARCHAR(255),
  `gallery` TEXT,
  `active` INT,
  `stock` INT
);

CREATE INDEX idx_product_code ON product (`code`);

-- PRODUCT COMBINATIONS TABLE

CREATE TABLE product_combination (
  `id` INT AUTO_INCREMENT PRIMARY KEY,
  `product_a_code` VARCHAR(255) NOT NULL,
  `product_b_code` VARCHAR(255) NOT NULL,
  `discount` DECIMAL(5, 2) DEFAULT 0,
  `created_at` TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  FOREIGN KEY (`product_a_code`) REFERENCES `product` (`code`),
  FOREIGN KEY (`product_b_code`) REFERENCES `product` (`code`)
);
