import React, {useEffect, useState} from 'react';
import {ethers} from 'ethers';

import {contractABI, contractAddress} from '../utils/constants';

export const TransactionContext = React.createContext();

//ethereum object given from metamask browser extension
const {ethereum} = window;

const getEthereumContract = () => {
    const provider = new ethers.providers.Web3Provider(ethereum);
    const signer = provider.getSigner();
    //fetches transaction contract
    const transactionContract = new ethers.Contract(contractAddress,contractABI,signer);

    return transactionContract;
}

//every context provider needs children from the props
export const TransactionProvider = ({children}) => {
    /*creates a state variable initially set blank (false) that is called currentAccount and the state of it can be modified with setCurrentAccount
        this variable is then accessible to Welcome.jsx through passing it in to TransactionContext.Provider
    */
    const [currentAccount, setCurrentAccount] = useState('');
    const [formData, setFormData] = useState({addressTo: '', amount: '', keyword: '', message: ''});
    const [isLoading, setIsLoading] = useState(false);
    const [transactionCount, setTransactionCount] = useState(localStorage.getItem('transactionCount'));
    const [transactions, setTransactions] = useState([]);

    //e is event being passed in
    const handleChange = (e, name) => {
        //when updating new state using old state we must use callback function with prevstate
        //updates the formData state with new form data
        setFormData((prevstate) => ({...prevstate, [name]: e.target.value}));
    }

    const getAllTransactions = async () => {
        try {
            if(!ethereum) return alert("Please install Metamask");
            const transactionContract = getEthereumContract();

            const availableTransactions = await transactionContract.getAllTransactions();

            const structuredTransactions = availableTransactions.map((transaction) => ({
                addressTo: transaction.receiver,
                addressFrom: transaction.sender,
                timestamp: new Date(transaction.timestamp.toNumber() * 1000).toLocaleString(),
                message: transaction.message,
                keyword: transaction.keyword,
                amount: parseInt(transaction.amount._hex) / (10 ** 18)
            }))
            console.log(structuredTransactions);

            setTransactions(structuredTransactions);
        } catch (error) {
            console.log(error);
        }
    }

    /*async functions are only used for calling promise methods which need to be awaited. Using async enables us to use await. No reason to make a function async unless you're calling a promise method. */
    const checkIfWalletIsConnected = async () => {
        try {
            if(!ethereum) return alert("Please install Metamask");

            const accounts = await ethereum.request({method: 'eth_accounts'});

            if(accounts.length){
                setCurrentAccount(accounts[0]);

                getAllTransactions();
            }else{
                console.log("No accounts found");
            }

            console.log(accounts);
        } catch (error) {
            console.log(error);

            throw new Error("No ethereum object.");
        }

    }

    const checkIfTransactionsExist = async () => {
        try {
            const transactionContract = getEthereumContract();
            const transactionCount = await transactionContract.getTransactionCount();

            window.localStorage.setItem("transactionCount", transactionCount);
        } catch (error) {
            console.log(error);

            throw new Error("No ethereum object.");
        }
    }

    const connectWallet = async () => {
        try {
            if(!ethereum) return alert("Please install Metamask");

            const accounts = await ethereum.request({method: 'eth_requestAccounts'});

            setCurrentAccount(accounts[0]);
        } catch (error) {
            console.log(error);

            throw new Error("No ethereum object.");
        }
    }

    const sendTransaction = async () => {
        try {
            if(!ethereum) return alert("Please install Metamask");
            //destructs form data
            const {addressTo, amount, keyword, message} = formData;
            const transactionContract = getEthereumContract();
            //converts our form decimal amount to hexadecimal because ethereum is technically in hex
            const parsedAmount = ethers.utils.parseEther(amount);

            await ethereum.request({
                method: 'eth_sendTransaction',
                params: [{
                    from: currentAccount,
                    to: addressTo,
                    gas: '0x5208', //21000 gwei
                    value: parsedAmount._hex //0.0001
                }]
            });

            const transactionHash = await transactionContract.addToBlockchain(addressTo, parsedAmount, message, keyword);


            setIsLoading(true);
            console.log(`Loading - ${transactionHash.hash}`);
            await transactionHash.wait();
            setIsLoading(false);
            console.log(`Success - ${transactionHash.hash}`);

            const transactionCount = await transactionContract.getTransactionCount();

            setTransactionCount(transactionCount.toNumber());
        } catch (error) {
            console.log(error);

            throw new Error("No ethereum object.");
        }
    }

    /*By using this Hook, you tell React that your component needs to do something after render. React will remember the function you passed (we’ll refer to it as our “effect”), and call it later after performing the DOM updates.
    In this effect, we set the document title, but we could also perform data fetching or call some other imperative API. */
    //use effect is needed to call the checkIfWallet is connected. This is only ran when our application loads.
    useEffect(() => {
      checkIfWalletIsConnected();
      checkIfTransactionsExist();
    }, []);


    //every context provider always has to return something
    return(
        //wraps entire react app with the data that gets passed into it
        //gives all our components access to connectWallet
        <TransactionContext.Provider value={{connectWallet, currentAccount, formData, setFormData, handleChange, sendTransaction, transactions, isLoading}}>
            {children}
        </TransactionContext.Provider>
    );
}

