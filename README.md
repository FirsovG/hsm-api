<img src="images/LOGO.JPG" alt="Logo" />

## Steel production API to simulate communication of a Hot Strip Mill.

[Check Docs](#docs) | [Create an issue](https://github.com/FirsovG/hsm-api/issues/new)

## Planned Functionality

<img src="images/STEPS_TELEGRAMS.JPG" alt="Planned Functionality" />

## Roadmap by functionality

The functionality will be implemented in the order listed below.

### Required

- [x] Production Start
- [ ] Production End
- [ ] Production Status
- [ ] Downtime
- [ ] Furnace Time Spend

### Nice to have

- [ ] Furnace Tempertaure Status
- [ ] Roughing Mill Dimensions Reduced To
- [ ] Finishing Mill Dimensions Reduced To
- [ ] Water Usage Status
- [ ] Profile Temperature After
- [ ] Diamter And Length
- [ ] Roughing Mill Change Roll
- [ ] Finishing Mill Change Roll

## Docs

### All Messages

`Fields`

- Message Id - Unique identifier of an HTTP message
- Message Creation Date - Time of message creation

### Production Start

`Fields`

- Coil Id - Unique identifier of an hot band coil
- Production Start Date - Time of production start

`Flow`

<img src="images/START_PRODUCTION_FLOW.JPG" alt="Planned Functionality" />

## License

HSM API is made available under the [MIT License](https://github.com/FirsovG/hsm-api/blob/main/LICENSE).
