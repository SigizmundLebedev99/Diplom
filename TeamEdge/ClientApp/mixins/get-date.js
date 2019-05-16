export default{
    methods:{
        getDate(date){
            var temp = new Date(date);
            return `${temp.getDate()}.${temp.getMonth() + 1}.${temp.getFullYear()}`
        },
        getDateTime(date){
            var temp = new Date(date);
            return `${temp.getHours()}:${temp.getMinutes()} ${temp.getDate()}.${temp.getMonth() + 1}.${temp.getFullYear()}`
        }
    }
}