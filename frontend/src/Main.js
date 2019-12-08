import React from 'react';
import ReactDOM from 'react-dom';
import Block from './Component/block'

import { formatAmount } from './utils.js';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faHome, faBolt } from '@fortawesome/free-solid-svg-icons'

export class Main extends React.Component {
    constructor() {
        super();
        this.state = {
            blocks : [],
            walletBalance: 0
        }
    }

    componentDidMount() {
      //  fetch('https://localhost:44319/api/block/mine')
       //   .then(response => response.json())
        //  .then(data => console.log({data}));
         // .then(data => this.setState({ data }));
         this.UpdateWalletBalance();
      }

    UpdateWalletBalance(){
        var bal = 0;
        this.state.blocks.forEach(block => {
            console.log(block);
            block.Transactions.forEach(transaction => bal+=transaction.Amount)
        })

        this.setState({walletBalance:bal});
    }

    AddBlock(){
       
         this.setState(previousState => ({
            blocks: [...previousState.blocks, <Block/>]
        }));
    
        this.UpdateWalletBalance();
        console.log(this.setState.blocks);
    }

    render() {
    
        return (
            <section className="section">
                <div className="container">
                    <h1 className="title">
                        Blockchain Simulator
                    </h1>
                    <div className="columns">
                        <div className="column">
                            <h2 className="subtitle">
                                <span className="icon has-text-info is-medium"><FontAwesomeIcon icon={faBolt} size="sm" /></span>
                                 Current Blocks
                            </h2>
                            {this.state.blocks.map((block) => {return block})}
                            <br/>
                            <br/>
                            <button className="button is-link is-medium" onClick={()=>this.AddBlock()} >Create Block</button>
                        </div>
                            <div className="column">
                                <h2 className="subtitle">
                                    <span className="icon has-text-success is-medium"><FontAwesomeIcon icon={faHome} size="sm" /></span>
                                    Your Wallet
                            </h2>
                                <p>Balance:</p>
                                <p>&nbsp;</p>
                                   <div className="notification is-primary">
                                        <h1 className="title">{formatAmount(this.state.walletBalance, 0)}</h1>
                                    </div>
                               
                            </div>
                        </div>
                    </div>
            </section>
                )
            }
        }
