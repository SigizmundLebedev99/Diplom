import {mapGetters} from 'vuex'
export default{
    computed: {
        ...mapGetters({workItems:'project/workItems'}),
        currentWI(){
            return this.workItems.find(e=>e.code===this.$route.name && e.number == this.$route.params.number)
        }
    },
  }