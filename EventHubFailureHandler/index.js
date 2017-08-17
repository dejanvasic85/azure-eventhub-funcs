module.exports = function (context, eventHubMessages) {
    context.log(`JavaScript eventhub trigger function called for message array ${eventHubMessages}`);

    const logger = context;

    eventHubMessages.forEach(message => {
        logger.log('Parsing message', message);
        
        const data = JSON.parse(message);

        context.log(`Processed message`, data);

        if (data.success === 'True') {
            console.log("And it's a success!");
            return Promise.resolve(true);
        }

        throw new Error('And its a Boom fail. :(');

    });

    context.done();
};