import { List } from '../common/List';
import { UserDetails} from '../app/components/watch/watch.service';

describe("list tests", function () {

    it("adds an element to a list", function () {
        var test = new List<UserDetails>();
        var t: UserDetails = {
            connectionId: '1',
            name: 'test1'
        }

        var t2: UserDetails = {
            connectionId: '2',
            name: 'test2'
        }

        test.push(t);
        test.push(t2);

        //Purpose is to test Symbol.iterator
        for (let t2 in test) {
            console.log(t2)
        }
    });
});