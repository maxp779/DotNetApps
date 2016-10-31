import { Button,Modal } from 'react-bootstrap';
import React from 'react';

class AboutPageModal extends React.Component{

    constructor() {
        super();
        this.close = this.close.bind(this);
        this.open = this.open.bind(this);

        this.state = { showModal: false };
    }

    close() {
        this.setState({ showModal: false });
    }

    open() {
        this.setState({ showModal: true });
    }

    render() {

        return (
<div>
<Button
    bsStyle="default"
    onClick={this.open}
    style={{"float":"right"}}>
    About this site
</Button>
    
    <Modal show={this.state.showModal} onHide={this.close}>
        <Modal.Header closeButton>
            <Modal.Title>About countdown helper</Modal.Title>
        </Modal.Header>
        <Modal.Body>
            <p>This site was built with react.js and react-bootstrap. As a result it is fully responsive and should work fine on mobiles/tablets!
            It was created so I could try out react and also .NET Core. Plus im pretty bad at countdown so it helps there too.</p>
            <hr />
            <h4>Created by Maximilian Power</h4>
            <a href="http://www.maxpower.rocks/">http://www.maxpower.rocks/</a>
        </Modal.Body>
        <Modal.Footer>
            <Button onClick={this.close}>Close</Button>
        </Modal.Footer>
    </Modal>
</div>
  );
    }
}

export {AboutPageModal}