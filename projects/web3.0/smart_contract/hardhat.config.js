// https://eth-ropsten.alchemyapi.io/v2/DgiIVXWiXnBbjKAorSYx4uf4jhCurRD1

require("dotenv").config();
//plugin to build smart contract tests
require('@nomiclabs/hardhat-waffle');

//url is given from making a new app on Alchemy site. Account is our wallet address that we want to use in order to fund the gas price
module.exports = {
  solidity: '0.8.0',
  networks: {
    ropsten: {
      url: 'https://eth-ropsten.alchemyapi.io/v2/5gfYvimCCxhNpLyCpfg7meJ8xYKK_Ow6',
      accounts: ['768ff57179c39fbf10b7e30b02ad03b03673a7bde586170e137a4ee32e94575a']
    }
  }
}