import abi from './Transactions.json';
//the abi is generated when we deploy the contract and is originally found in smart_contract> contracts > transactions.json
export const contractABI = abi.abi;
//this address is generated when we deploy our contract using hardhat
export const contractAddress = '0xe1D66266c81df6F7bdb3cd3a0c5a121815b9D356';