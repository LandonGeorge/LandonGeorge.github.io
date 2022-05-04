//SPDX-License-Identifier: UNLICENSED
pragma solidity ^0.8.0;

//contract acts like a class
contract Transactions {
    uint256 transactionCount;


    //!!!!!!!!!!!!!!!!!!!DONT MAKE TYPOS ANYWHERE IN SMART CONTRACTS BECAUSE IT COSTS A LOT OF MONEY!!!!!!!!!!!!!!!!!!!!!!
    //IF YOU MAKE A MISTAKE GO TO ALCHEMY > NEW APP > GET URL > put it in hardhat config > redeploy the contract and update the address to the contract and the json file in utils/transactions.json
    //function that we'll emit or call later
    event Transfer(address from, address receiver, uint amount, string message, uint256 timestamp, string keyword);

    //similiar to an object
    struct TransferStruct {
        //properties of the object
        address sender;
        address receiver;
        uint amount;
        string message;
        uint256 timestamp;
        string keyword;
    }

    //array of transferstructs
    TransferStruct[] transactions;


    function addToBlockchain(address payable receiver, uint amount, string memory message, string memory keyword) public{
        transactionCount += 1;
        //only adds our transaction to the array, doesn't actually make the transaction on the blockchain
        transactions.push(TransferStruct(msg.sender, receiver, amount, message, block.timestamp, keyword));

        //executes the transaction on the blockchain
        emit Transfer(msg.sender, receiver, amount, message, block.timestamp, keyword);
    }

    function getAllTransactions() public view returns (TransferStruct[] memory){
        return transactions;
    }

    function getTransactionCount() public view returns (uint256){
        return transactionCount;
    }
}