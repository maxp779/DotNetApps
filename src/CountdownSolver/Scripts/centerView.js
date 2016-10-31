import React from 'react';
import { Grid,Row,Col } from 'react-bootstrap';

class CenterView extends React.Component {
    render() {
        return (
            <Grid>
                <Row className="show-grid">
                    <Col xs={3} md={3}></Col>
                    <Col xs={6} md={6}>{this.props.children}</Col>
                    <Col xs={3} md={3}></Col>
                </Row>
            </Grid>
        )
    }
}

export {CenterView}