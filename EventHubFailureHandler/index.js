module.exports = function (context, eventHubMessages) {
    context.log(`JavaScript eventhub trigger function called for message array ${eventHubMessages}`);

    const logger = context;

    eventHubMessages.forEach(message => {
        context.log(`Processed message`, message);

        if (message.success === true) {
            console.log("And it's a success!");
            return Promise.resolve(true);
        }

        throw new Error('And its a Boom fail. :(');

    });

    context.done();
};