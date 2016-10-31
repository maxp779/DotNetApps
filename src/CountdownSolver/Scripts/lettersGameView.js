import { Panel,Button,FormGroup,ControlLabel,FormControl,InputGroup,Row,Col } from 'react-bootstrap';
import React from 'react';

class LettersGameView extends React.Component {
    
    render() {
        let loadingSpinner;
        if (this.props.loading) {
            loadingSpinner = <div className="loader"></div>;
        }
        else{
            loadingSpinner = "";
        }
        return (
    <Panel header="Countdown letters helper" bsStyle="primary">
        <Row>
            <Col xs={12} md={6}>
                <form onSubmit={this.props.handleSubmit}>
                    <FormGroup>
                        <ControlLabel>Enter letters</ControlLabel>
                            <InputGroup>
                                <FormControl
                                    type="text"
                                    required="required"
                                    value={this.props.currentLetters}
                                    placeholder="e.g 'elomsftpg'"
                                    onChange={this.props.handleChange}
                                        />
                            <InputGroup.Button>
                                <Button bsStyle="primary" type="submit">Search</Button>
                                <Button bsStyle="danger" type="button" onClick={this.props.handleReset}>Reset</Button>
                            </InputGroup.Button>
                        </InputGroup>
                    </FormGroup>
                </form>
            </Col>
        <Col xs={12} md={6} style={{"float":"right"}}>
            <Panel header="Words" bsStyle="info">
                <Row>
                    <Col xs={12}>
            {this.props.lettersMessage}
                    </Col>
                </Row>
            <Row>
                <Col xs={12}>
            {loadingSpinner}
                </Col>
            </Row>
            <ul className="scrollableList">
            {this.props.wordsList.map(function(listValue){

                let decriptiveWord = "letters"
                if(listValue.length === 1)
                {
                    decriptiveWord = "letter";
                }

                return <li key={listValue}>{listValue} [{listValue.length} {decriptiveWord}]</li>;
            })}
            </ul>
        </Panel>
    </Col>
        </Row>
    </Panel>
      );
    }
}

export {LettersGameView}