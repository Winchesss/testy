# Restful Booker API Tests

This project contains API tests created in Postman for the Restful Booker API.

## Tested API

Restful Booker API: `https://restful-booker.herokuapp.com`

## Project scope

The purpose of this project is to test basic CRUD operations for booking management.

Tested operations:

* Create authentication token
* Create a booking
* Get booking details
* Update a booking
* Delete a booking

## Tools used

* Postman
* JavaScript assertions in Postman

## Collection structure

The Postman collection contains the following requests:

1. `Auth - Create Token`
2. `Create Booking`
3. `Get Booking`
4. `Update Booking`
5. `Delete Booking`

## What is tested

The tests check:

* HTTP status codes
* Response body structure
* Correct values in the response
* Saving `bookingId` as an environment variable
* Updating booking data
* Deleting a booking

## How to run the tests

1. Download this repository.

2. Open Postman.

3. Import the collection from:

   `postman/Restful_Booker_API_Tests.json`

4. Import the environment from:

   `postman/Restful_Booker_Environment.json`

5. Select the imported environment in Postman.

6. Run the requests in this order:

   1. `Auth - Create Token`
   2. `Create Booking`
   3. `Get Booking`
   4. `Update Booking`
   5. `Delete Booking`

## Notes

The API is public and used for testing purposes.
Booking data may sometimes disappear or change because the API is shared by many users.
