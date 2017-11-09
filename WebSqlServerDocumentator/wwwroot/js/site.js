// Write your JavaScript code.

Vue.component('documented-server', {
    props: {
        name: { type: String, default: '' },
        displayName: { type: String, default: '' },
        description: { type: String, default: '' },
        url: { type: String, default: '' }
    },
    template: '<div>{{displayName}} - {{name}} || {{description}} || <a :href="url">Explore</a></div>'
});

new Vue(
    {
        el: '.servers'
    });