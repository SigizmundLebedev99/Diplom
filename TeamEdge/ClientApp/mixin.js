import Vue from 'vue'

Vue.mixin({
  methods:{
    resize(sizes){
      var size = sizes[this.$vuetify.breakpoint.name];
      if(!size)
        return undefined;
      return size;
    }
  }
})
