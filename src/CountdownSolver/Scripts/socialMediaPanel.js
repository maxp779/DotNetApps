import { Panel,Row,Col } from 'react-bootstrap';
import React from 'react';

class SocialMediaPanel extends React.Component {

    render()
    {
        return(
        <Panel>
            <Row>
                <Col xs={12}>
                    <p>Share this site on:</p>
                    <div className="addthis_inline_share_toolbox"></div> 
                </Col>
            </Row>
        </Panel>)
    }

}

export {SocialMediaPanel};